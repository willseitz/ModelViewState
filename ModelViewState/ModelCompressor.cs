using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace OpenSource.ModelViewState
{
	internal class ModelCompressor
	{
		private static readonly byte[] Salt = Encoding.ASCII.GetBytes("StolenFromStackOverflow");

		public static string Compress(object model)
		{
			byte[] byteArray;
			using (AesManaged aesAlg = new AesManaged())
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(model.GetType().ToString(), Salt);
				aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

				// Create a decryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					// prepend the IV
					memoryStream.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
					memoryStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);
					using (CryptoStream encryptStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (GZipStream gZipStream = new GZipStream(encryptStream, CompressionMode.Compress))
						{
							Serialize(model, gZipStream);
						}
					}
					byteArray = memoryStream.ToArray();
				}
			}
			return Convert.ToBase64String(byteArray);
		}

		public static object Decompress(Type modelType, string model)
		{
			Object retval;
			using (AesManaged aesAlg = new AesManaged())
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(modelType.ToString(), Salt);
				using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(model)))
				{
					// Create a RijndaelManaged object
					// with the specified key and IV.
					aesAlg.Key = key.GetBytes(aesAlg.KeySize/8);
					// Get the initialization vector from the encrypted stream
					aesAlg.IV = ReadByteArray(memoryStream);
					// Create a decrytor to perform the stream transform.
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					using (CryptoStream decryptStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (GZipStream gZipStream = new GZipStream(decryptStream, CompressionMode.Decompress))
						{
							retval = Deserialize(modelType, gZipStream);
						}
					}
				}
			}
			return retval;
		}

		private static void Serialize(Object obj, Stream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, obj);
		}
		private static object Deserialize(Type modelType, Stream stream)
		{
			IFormatter formatter = new BinaryFormatter();
			return Convert.ChangeType(formatter.Deserialize(stream), modelType); 
		}
		private static byte[] ReadByteArray(Stream s)
		{
			byte[] rawLength = new byte[sizeof(int)];
			if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
			{
				throw new SystemException("Stream did not contain properly formatted byte array");
			}

			byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
			if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new SystemException("Did not read byte array properly");
			}

			return buffer;
		}
	}

}
