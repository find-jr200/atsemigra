using System;
using System.Text;
using System.Collections.Generic;


namespace atsemigra
{
	/// <summary>
	/// CJR形式データを格納
	/// </summary>
	public class CjrFormat
	{
        public enum BaudRate { Low = 0x64, High = 0 };
        private List<byte> cjrData = null;
        private int blockCount = 0;
        private int startAddress = 0;
        private bool isClosed = false;

        public CjrFormat() { }

        /// <summary>
        /// CJRバイト配列がすでにある時のコンストラクタ
        /// </summary>
        /// <param name="array">CJRバイト配列</param>
        public CjrFormat(byte[] array)
        {
            cjrData = new List<byte>(array);
            isClosed = true;
        }

        /// <summary>
        /// CJRのバイト配列を取得
        /// </summary>
        /// <returns>配列</returns>
        public byte[] GetCjrData()
        {
            if (cjrData == null)
            {
                return null;

            } else
            {
                if (!isClosed) { 
                    CloseCjrData();
                }
                return cjrData.ToArray();
            }
        }


        /// <summary>
        /// ボーレート指定を変更
        /// </summary>
        public void SetBaudRate(BaudRate val)
        {
            cjrData[23] = (byte)val;
            byte sum = 0;
            for (int i = 0; i < 32; i++) sum += cjrData[i];
            cjrData[32] = sum;
        }


        /// <summary>
        /// CJRの先頭ブロックのスタートアドレスを取得
        /// マルチタイプのCJRには無効
        /// </summary>
        /// <returns>スタートアドレス。エラー時は-1</returns>
        public int GetStartAddress()
        {
            int retVal = 0;

            if (cjrData == null)
            {
                retVal = -1;
            } else
            {
                retVal = cjrData[37];
                retVal <<= 8;
                retVal += cjrData[38];
            }

            return retVal;
        }


        /// <summary>
        /// CJR形式をBIN形式に変換
        /// </summary>
        /// <returns>BIN形式のbyte配列</returns>
        public byte[] GetBinData()
		{
            List<byte> list = new List<byte>(32768);
			int blockSize = 0;
			int currentPoint = 0;

            if (cjrData == null)
                return null;

			// ヘッダ読み飛ばし
			currentPoint = 6 + cjrData[3] + 1;

			// データブロック・フッタブロック読み出し
			while (cjrData[currentPoint + 2] != (byte)0xff)
			{
				blockSize = cjrData[currentPoint + 3] == 0 ? 256 : cjrData[currentPoint + 3];
				currentPoint += 6;
				for (int i = 0; i < blockSize; i++) 
				{
					list.Add((byte)cjrData[currentPoint++]);
				}
				currentPoint++;
			}
			
			return list.ToArray();
		}

        /// <summary>
        /// BINをCJRに変換
        /// Appendすることで離れたアドレスへのロードをまとめられる
        /// Append時はJR用ファイル名はダミー（不使用）
        /// </summary>
        /// <param name="binData">bin ファイル</param>
        /// <param name="jrFileName">JR-200用のファイル名</param>
        /// <param name="startAddress">マシン語の場合のスタートアドレス。BASIC時はダミー</param>
        /// <param name="basic">BASICならtrue</param>
        /// <param name="append">appendするときはtrue</param>
        /// <returns>成功したらtrue</returns>
		public bool AddBinData(byte[] binData, string jrFileName, int sAddress, bool basic, bool append = false)
		{
			int checkSum = 0, pointer = 0, blockHead = 0;
            byte[] binArray;

            if (binData == null || (append && isClosed))
                return false;

            startAddress = sAddress;
            if (basic) startAddress = 0x801;

            if (append && cjrData == null)
                append = false;
            
            if (!append)
            {
                blockCount = 0;
                cjrData = new List<byte>(32768);
                isClosed = false;

                // ヘッダブロック
                cjrData.Add(2);
                cjrData.Add(0x2a);
                cjrData.Add((byte)blockCount);
                cjrData.Add(0x1a);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                byte[] bytes = sjisEnc.GetBytes(jrFileName);
                BArraySupport.AddBytesToList(cjrData, bytes); // ファイル名
                for (int i = 0; i < 16 - bytes.Length; i++) cjrData.Add(0);
                cjrData.Add((byte)(basic ? 0 : 1)); // マシン語 or BASIC
                cjrData.Add(0); // ボーレート

                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                cjrData.Add(0xff);

                for (int i = 0; i < cjrData.Count; i++)
                    checkSum += (int)cjrData[i];
                cjrData.Add((byte)checkSum);

                blockCount++;
            }

            // データブロック
            while (true)
			{
                blockHead = cjrData.Count;
				cjrData.Add(2);
				cjrData.Add(0x2a);
				cjrData.Add((byte)blockCount);
                cjrData.Add(0); // ダミー（ブロックサイズ）
                int sizePoint = cjrData.Count - 1;

                binArray = BitConverter.GetBytes(Convert.ToUInt16(startAddress));
                Array.Reverse(binArray);
                BArraySupport.AddBytesToList(cjrData, binArray);

                int j;
                for (j = 0; j < 256; j++) 
				{
                    cjrData.Add(binData[pointer]);
                    ++pointer;
                    if (pointer == binData.Length) break;
				}

                cjrData[sizePoint] = (byte)((j == 256) ? 0 : j + 1);
                
                // チェックサム計算
				checkSum = 0;
                for (int i = blockHead; i < cjrData.Count; i++)
                    checkSum += (int)cjrData[i];
                cjrData.Add((byte)checkSum);

                ++blockCount;
                startAddress += j;

                if (pointer == binData.Length) break;
			}

            return true;
		}




        /// <summary>
        // CJRファイルにフッタブロックを追加する
        /// </summary>
        public void CloseCjrData()
        {
            byte[] barray;

            // フッタブロック
            cjrData.Add(2);
            cjrData.Add(0x2a);
            cjrData.Add(0xff);
            cjrData.Add(0xff);
            int endAddress = startAddress + 1;
            if (endAddress == 0x10000) endAddress = 0;
            barray = BitConverter.GetBytes(Convert.ToUInt16(endAddress));
            Array.Reverse(barray);
            BArraySupport.AddBytesToList(cjrData, barray);

            isClosed = true;
        }
    }
}
