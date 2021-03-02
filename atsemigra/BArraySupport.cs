using System;
using System.Collections.Generic;


namespace atsemigra
{
	/// <summary>
	/// byte[]�^�z��̔�r�A���������郁�\�b�h���W�߂��T�|�[�g�N���X
	/// </summary>
	public static class BArraySupport
	{
        /// <summary>
        /// byte�^�z��̗v�f��r
        /// </summary>
        /// <param name="a">��r�v�fa</param>
        /// <param name="b">��r�v�fb</param>
        /// <returns>��v�����true</returns>
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
		/// list<byte>��byte[]��ǉ�
		/// </summary>
		/// <param name="list">�ǉ������List<byte></param>
		/// <param name="bytes">�ǉ�����byte[]</param>
        public static void AddBytesToList(List<byte> list, byte[] bytes)
		{
			for (int i = 0; i < bytes.Length; i++) 
			{
				list.Add(bytes[i]);
			}
		}



	}
}
