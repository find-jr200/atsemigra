using System;
using System.Text;
using System.Collections.Generic;


namespace atsemigra
{
	/// <summary>
	/// CJR�`���f�[�^���i�[
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
        /// CJR�o�C�g�z�񂪂��łɂ��鎞�̃R���X�g���N�^
        /// </summary>
        /// <param name="array">CJR�o�C�g�z��</param>
        public CjrFormat(byte[] array)
        {
            cjrData = new List<byte>(array);
            isClosed = true;
        }

        /// <summary>
        /// CJR�̃o�C�g�z����擾
        /// </summary>
        /// <returns>�z��</returns>
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
        /// �{�[���[�g�w���ύX
        /// </summary>
        public void SetBaudRate(BaudRate val)
        {
            cjrData[23] = (byte)val;
            byte sum = 0;
            for (int i = 0; i < 32; i++) sum += cjrData[i];
            cjrData[32] = sum;
        }


        /// <summary>
        /// CJR�̐擪�u���b�N�̃X�^�[�g�A�h���X���擾
        /// �}���`�^�C�v��CJR�ɂ͖���
        /// </summary>
        /// <returns>�X�^�[�g�A�h���X�B�G���[����-1</returns>
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
        /// CJR�`����BIN�`���ɕϊ�
        /// </summary>
        /// <returns>BIN�`����byte�z��</returns>
        public byte[] GetBinData()
		{
            List<byte> list = new List<byte>(32768);
			int blockSize = 0;
			int currentPoint = 0;

            if (cjrData == null)
                return null;

			// �w�b�_�ǂݔ�΂�
			currentPoint = 6 + cjrData[3] + 1;

			// �f�[�^�u���b�N�E�t�b�^�u���b�N�ǂݏo��
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
        /// BIN��CJR�ɕϊ�
        /// Append���邱�Ƃŗ��ꂽ�A�h���X�ւ̃��[�h���܂Ƃ߂���
        /// Append����JR�p�t�@�C�����̓_�~�[�i�s�g�p�j
        /// </summary>
        /// <param name="binData">bin �t�@�C��</param>
        /// <param name="jrFileName">JR-200�p�̃t�@�C����</param>
        /// <param name="startAddress">�}�V����̏ꍇ�̃X�^�[�g�A�h���X�BBASIC���̓_�~�[</param>
        /// <param name="basic">BASIC�Ȃ�true</param>
        /// <param name="append">append����Ƃ���true</param>
        /// <returns>����������true</returns>
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

                // �w�b�_�u���b�N
                cjrData.Add(2);
                cjrData.Add(0x2a);
                cjrData.Add((byte)blockCount);
                cjrData.Add(0x1a);
                cjrData.Add(0xff);
                cjrData.Add(0xff);
                Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
                byte[] bytes = sjisEnc.GetBytes(jrFileName);
                BArraySupport.AddBytesToList(cjrData, bytes); // �t�@�C����
                for (int i = 0; i < 16 - bytes.Length; i++) cjrData.Add(0);
                cjrData.Add((byte)(basic ? 0 : 1)); // �}�V���� or BASIC
                cjrData.Add(0); // �{�[���[�g

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

            // �f�[�^�u���b�N
            while (true)
			{
                blockHead = cjrData.Count;
				cjrData.Add(2);
				cjrData.Add(0x2a);
				cjrData.Add((byte)blockCount);
                cjrData.Add(0); // �_�~�[�i�u���b�N�T�C�Y�j
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
                
                // �`�F�b�N�T���v�Z
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
        // CJR�t�@�C���Ƀt�b�^�u���b�N��ǉ�����
        /// </summary>
        public void CloseCjrData()
        {
            byte[] barray;

            // �t�b�^�u���b�N
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
