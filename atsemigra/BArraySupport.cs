using System;
using System.Collections.Generic;


namespace atsemigra
{
	/// <summary>
	/// byte[]型配列の比較、結合をするメソッドを集めたサポートクラス
	/// </summary>
	public static class BArraySupport
	{
        /// <summary>
        /// byte型配列の要素比較
        /// </summary>
        /// <param name="a">比較要素a</param>
        /// <param name="b">比較要素b</param>
        /// <returns>一致すればtrue</returns>
		public static bool CompareByteArray(byte[] a, byte[] b)
		{
			bool retVal = true;

			if (a.Length != b.Length) 
			{
				retVal = false;
			} 
			else 
			{
				for (int i = 0; i < a.Length; i++) 
				{
					if (a[i] != b[i]) 
					{
						retVal = false;
					}
				}
			}
			return retVal;
		}

		/// <summary>
		/// list<byte>にbyte[]を追加
		/// </summary>
		/// <param name="list">追加されるList<byte></param>
		/// <param name="bytes">追加するbyte[]</param>
        public static void AddBytesToList(List<byte> list, byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++) 
			{
				list.Add(bytes[i]);
			}
		}



	}
}
