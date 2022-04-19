using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace CodingAssignment
{
	class Program
	{
		static void Main(string[] args)
		{
			Debug.WriteLine("Coding Assignment (Linear Algebra - Matrix Encryption) - Jei Yang");
			Debug.WriteLine("-------------------------------------------");
			var matrix2x2 = new Matrix4x4(new Matrix3x2(4, 1, 2, 1, 0, 0)); // 2x2 matrix K
			Debug.WriteLine("2x2 example matrix - " + matrix2x2.ToString());
            Debug.WriteLine("Determinant is - " + matrix2x2.GetDeterminant());

            // Question 1. Create matrix K and its inverse.
            var matrix2x2_inv = GetInverseMatrixMod27(matrix2x2, 2); 
			Debug.WriteLine("Inverse of 2x2 example matrix  - " + matrix2x2_inv.ToString());

			// Question 2~6. Break a message into strings of length n (= 2)
			var encryptedMessage = Encrypt("WHERE DID MY WEEEKEND GO", matrix2x2, 2);
            Debug.WriteLine("(Encrpytion) WHERE DID MY WEEEKEND GO -> '" + encryptedMessage + "'");

            // Question 7~9. Decode the plaintext using the inverse matrix.
            var decryptedMessage = Decrypt(encryptedMessage, matrix2x2_inv, 2);
            Debug.WriteLine("(Decryption) " + encryptedMessage + " -> '" + decryptedMessage + "'");


            Debug.WriteLine("-------------------------------------------");
            var matrix4x4 = new Matrix4x4(3, 5, 1, 8, 1, 7, 3, 1, 9, 6, 2, 1, 2, 1, 3, 1); // 4x4 matrix K
            var matrix4x4_v2 = new Matrix4x4(3, 5, 1, 6, 1, 1, 3, 2, 1, 1, 1, 1, 2, 1, 0, 1); // 4x4 matrix K

            Debug.WriteLine("4x4 example matrix - " + matrix4x4.ToString());
            Debug.WriteLine("Determinant is - " + matrix4x4.GetDeterminant());
            // Question 1. Create matrix K and its inverse.
            var matrix4x4_inv = GetInverseMatrixMod27(matrix4x4, 4);
            Debug.WriteLine("Inverse of 4x4 example matrix  - " + matrix4x4_inv.ToString());

            // Question 2~6. Break a message into strings of length n (= 4)
            encryptedMessage = Encrypt("(Encrpytion) Please keep the door open", matrix4x4, 4);
            Debug.WriteLine("Please keep the door open -> '" + encryptedMessage + "'");

            // Question 7~9. Decode the plaintext using the inverse matrix.
            decryptedMessage = Decrypt(encryptedMessage, matrix4x4_inv, 4);
            Debug.WriteLine("(Decryption) " + encryptedMessage + " -> '" + decryptedMessage + "'");
        }
		private static string Encrypt(string message, Matrix4x4 matrix, int n)
		{
            while (message.Length % n != 0)
                message += " ";

            var utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(message.ToUpper());

            var messageCharArray = message.ToUpper().ToCharArray();
            int[] encodedInts = new int[message.Length];

			var arrayLength = (int) Math.Ceiling((message.Length / (double) n));
			int[][] matrixArray = new int[arrayLength][];

			for (int i = 0; i < arrayLength; i++)
			{
                if (n == 2)
                {
                    matrixArray[i] = new int[2] { 0, 0 };
                }
                else if (n == 4)
                {
                    matrixArray[i] = new int[4] { 0, 0, 0, 0 };
                }

                for (int j = 0; j < n; j++)
                {
                    encodedInts[(i * n) + j] = (encodedBytes[(i * n) + j] == 32) ? 0 : (encodedBytes[(i * n) + j] - 'A' + 1);
                    Debug.Write(messageCharArray[(i * n) + j] + "(" + encodedInts[(i * n) + j] + ")  ");
                }
                
                Debug.WriteLine("");
			}


            int[][] matrixArray_mod27 = new int[arrayLength][];

            if (n == 2)
			{
                Debug.WriteLine("------------- n == 2 -----------------");
                
                // Question 4 - 5
                for (int i = 0; i < arrayLength; i++)
                {
                    //var row1_convertedByKey = Convert.ToInt32((matrix.M11 * matrixArray[i][0]) + (matrix.M12 * matrixArray[i][1]));
                    var row1_convertedByKey = Convert.ToInt32((matrix.M11 * encodedInts[(i * 2)]) + (matrix.M12 * encodedInts[(i * 2) + 1]));
                    var row1_convertedToMod27 = (row1_convertedByKey > 0) ? (row1_convertedByKey % 27) : (row1_convertedByKey % 27) + 27;

                    //var row2_convertedByKey = Convert.ToInt32((matrix.M21 * matrixArray[i][0]) + (matrix.M22 * matrixArray[i][1]));
                    var row2_convertedByKey = Convert.ToInt32((matrix.M21 * encodedInts[(i * 2)]) + (matrix.M22 * encodedInts[(i * 2) + 1]));
                    var row2_convertedToMod27 = (row2_convertedByKey > 0) ? (row2_convertedByKey % 27) : (row2_convertedByKey % 27) + 27;

                    matrixArray_mod27[i] = new int[] { row1_convertedToMod27, row2_convertedToMod27 };
                    //Debug.WriteLine(matrixArray_mod27[i][0] + "\t" + matrixArray_mod27[i][1]);
                }
            }
            else if (n == 4)
            {
                Debug.WriteLine("------------- n == 4 -----------------");

                // Question 4 - 5
                for (int i = 0; i < arrayLength; i++)
                {
                    var row1_convertedByKey = Convert.ToInt32((matrix.M11 * encodedInts[(i * 4)]) + (matrix.M12 * encodedInts[(i * 4) + 1]) + (matrix.M13 * encodedInts[(i * 4) + 2]) + (matrix.M14 * encodedInts[(i * 4) + 3]));
                    var row1_convertedToMod27 = (row1_convertedByKey > 0) ? (row1_convertedByKey % 27) : (row1_convertedByKey % 27) + 27;
                    
                    var row2_convertedByKey = Convert.ToInt32((matrix.M21 * encodedInts[(i * 4)]) + (matrix.M22 * encodedInts[(i * 4) + 1]) + (matrix.M23 * encodedInts[(i * 4) + 2]) + (matrix.M24 * encodedInts[(i * 4) + 3]));
                    var row2_convertedToMod27 = (row2_convertedByKey > 0) ? (row2_convertedByKey % 27) : (row2_convertedByKey % 27) + 27;

                    var row3_cnvertedByKey = Convert.ToInt32((matrix.M31 * encodedInts[(i * 4)]) + (matrix.M32 * encodedInts[(i * 4) + 1]) + (matrix.M33 * encodedInts[(i * 4) + 2]) + (matrix.M34 * encodedInts[(i * 4) + 3]));
                    var row3_convertedToMod27 = (row3_cnvertedByKey > 0) ? (row3_cnvertedByKey % 27) : (row3_cnvertedByKey % 27) + 27;

                    var row4_convertedByKey = Convert.ToInt32((matrix.M41 * encodedInts[(i * 4)]) + (matrix.M42 * encodedInts[(i * 4) + 1]) + (matrix.M43 * encodedInts[(i * 4) + 2]) + (matrix.M44 * encodedInts[(i * 4) + 3]));
                    var row4_convertedToMod27 = (row4_convertedByKey > 0) ? (row4_convertedByKey % 27) : (row4_convertedByKey % 27) + 27;

                    matrixArray_mod27[i] = new int[] { row1_convertedToMod27, row2_convertedToMod27, row3_convertedToMod27, row4_convertedToMod27 };
                    //Debug.WriteLine(matrixArray_mod27[i][0] + "\t" + matrixArray_mod27[i][1] + "\t" + matrixArray_mod27[i][2] + "\t" + matrixArray_mod27[i][3]);
                }
            }

            // Question 6. Convert this back to chracters (plaintext)
            for (int k = 0; k < message.Length; k++)
            {
                int c = matrixArray_mod27[k / n][k % n];
                byte dc = c == 0 ? ((byte)'-') : Convert.ToByte(c + 'A' - 1);
                encodedBytes[k] = dc;// Convert.ToByte(matrixArray_mod27[k/2][k%2] + 'A' - 1); // + 64. for 0, add 32.
            }
            var plaintext = utf8.GetString(encodedBytes);
            //Debug.WriteLine("Text: " + plaintext);

            return plaintext;
		}


        private static string Decrypt(string message, Matrix4x4 matrix, int n)
        {
            var utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(message.ToUpper());
            var messageCharArray = message.ToUpper().ToCharArray();
            int[] encodedInts = new int[message.Length];

            // Question 7.
            for (int i = 0; i < message.Length; i++)
            {
                encodedInts[i] = (encodedBytes[i] == 45) ? 0 : (encodedBytes[i] - 'A' + 1);
                //Debug.Write(messageCharArray[i] + "(" + encodedInts[i] + ")  ");
            }

            var arrayLength = (int)Math.Ceiling((message.Length / (double)n));
            int[][] matrixArray_mod27 = new int[arrayLength][];
            Debug.WriteLine("");

            if (n == 2)
            {
                Debug.WriteLine("------------- n == 2 -----------------");
                
                for (int i = 0; i < arrayLength; i++)
                {
                    //var row1_convertedByKey = Convert.ToInt32((matrix.M11 * matrixArray[i][0]) + (matrix.M12 * matrixArray[i][1]));
                    var row1_convertedByKey = Convert.ToInt32((matrix.M11 * encodedInts[(i * 2)]) + (matrix.M12 * encodedInts[(i * 2) + 1]));
                    var row1_convertedToMod27 = (row1_convertedByKey > 0) ? (row1_convertedByKey % 27) : (row1_convertedByKey % 27) + 27;

                    //var row2_convertedByKey = Convert.ToInt32((matrix.M21 * matrixArray[i][0]) + (matrix.M22 * matrixArray[i][1]));
                    var row2_convertedByKey = Convert.ToInt32((matrix.M21 * encodedInts[(i * 2)]) + (matrix.M22 * encodedInts[(i * 2) + 1]));
                    var row2_convertedToMod27 = (row2_convertedByKey > 0) ? (row2_convertedByKey % 27) : (row2_convertedByKey % 27) + 27;

                    matrixArray_mod27[i] = new int[] { row1_convertedToMod27, row2_convertedToMod27 };
                    //Debug.WriteLine(row1_convertedByKey + "\t" + row2_convertedByKey); // Question 7. converted by key (70, 147, 410, ...)
                    //Debug.WriteLine(matrixArray_mod27[i][0] + "\t" + matrixArray_mod27[i][1]); // Question 8. back to mod27 (16, 12, 5, ...)
                }
            }
            else if (n == 4)
            {
                Debug.WriteLine("\n------------- n == 4 -----------------");

                // Question 4 - 5
                for (int i = 0; i < arrayLength; i++)
                {
                    var row1_convertedByKey = Convert.ToInt32((matrix.M11 * encodedInts[(i * 4)]) + (matrix.M12 * encodedInts[(i * 4) + 1]) + (matrix.M13 * encodedInts[(i * 4) + 2]) + (matrix.M14 * encodedInts[(i * 4) + 3]));
                    var row1_convertedToMod27 = (row1_convertedByKey > 0) ? (row1_convertedByKey % 27) : (row1_convertedByKey % 27) + 27;

                    var row2_convertedByKey = Convert.ToInt32((matrix.M21 * encodedInts[(i * 4)]) + (matrix.M22 * encodedInts[(i * 4) + 1]) + (matrix.M23 * encodedInts[(i * 4) + 2]) + (matrix.M24 * encodedInts[(i * 4) + 3]));
                    var row2_convertedToMod27 = (row2_convertedByKey > 0) ? (row2_convertedByKey % 27) : (row2_convertedByKey % 27) + 27;

                    var row3_cnvertedByKey = Convert.ToInt32((matrix.M31 * encodedInts[(i * 4)]) + (matrix.M32 * encodedInts[(i * 4) + 1]) + (matrix.M33 * encodedInts[(i * 4) + 2]) + (matrix.M34 * encodedInts[(i * 4) + 3]));
                    var row3_convertedToMod27 = (row3_cnvertedByKey > 0) ? (row3_cnvertedByKey % 27) : (row3_cnvertedByKey % 27) + 27;

                    var row4_convertedByKey = Convert.ToInt32((matrix.M41 * encodedInts[(i * 4)]) + (matrix.M42 * encodedInts[(i * 4) + 1]) + (matrix.M43 * encodedInts[(i * 4) + 2]) + (matrix.M44 * encodedInts[(i * 4) + 3]));
                    var row4_convertedToMod27 = (row4_convertedByKey > 0) ? (row4_convertedByKey % 27) : (row4_convertedByKey % 27) + 27;

                    matrixArray_mod27[i] = new int[] { row1_convertedToMod27, row2_convertedToMod27, row3_convertedToMod27, row4_convertedToMod27 };
                    //Debug.WriteLine(matrixArray_mod27[i][0] + "\t" + matrixArray_mod27[i][1] + "\t" + matrixArray_mod27[i][2] + "\t" + matrixArray_mod27[i][3]);
                }
            }

            // Question 9. back to plaintext
            for (int k = 0; k < message.Length; k++)
            {
                int c = matrixArray_mod27[k / n][k % n];
                byte dc = c == 0 ? ((byte)' ') : Convert.ToByte(c + 'A' - 1);
                encodedBytes[k] = dc;
            }
            var plaintext = utf8.GetString(encodedBytes);
            while (plaintext.EndsWith(' '))
            {
                plaintext = plaintext.Remove(plaintext.Length - 1);
            }

            return plaintext;
            
        }

        /*
		 *  Get the inverse matrix of the given 2x2 matrix, coverted to Mod27.
		 *  the matrix if considered 2x2 unless the given n value is 4.
		 */
        private static Matrix4x4 GetInverseMatrixMod27 (Matrix4x4 matrix, int n)
		{
			var convertedMatrix = new Matrix4x4();
			Matrix4x4.Invert(matrix, out convertedMatrix);
			var determinant = matrix.GetDeterminant();
			convertedMatrix = Matrix4x4.Multiply(convertedMatrix, determinant);

			convertedMatrix.M11 = GetInverseNumber(convertedMatrix.M11, determinant);
			convertedMatrix.M12 = GetInverseNumber(convertedMatrix.M12, determinant);
			convertedMatrix.M21 = GetInverseNumber(convertedMatrix.M21, determinant);
			convertedMatrix.M22 = GetInverseNumber(convertedMatrix.M22, determinant);
			if (n == 4)
			{
				convertedMatrix.M13 = GetInverseNumber(convertedMatrix.M13, determinant);
				convertedMatrix.M14 = GetInverseNumber(convertedMatrix.M14, determinant);
				convertedMatrix.M23 = GetInverseNumber(convertedMatrix.M23, determinant);
				convertedMatrix.M24 = GetInverseNumber(convertedMatrix.M24, determinant);
				convertedMatrix.M31 = GetInverseNumber(convertedMatrix.M31, determinant);
				convertedMatrix.M32 = GetInverseNumber(convertedMatrix.M32, determinant);
				convertedMatrix.M33 = GetInverseNumber(convertedMatrix.M33, determinant);
				convertedMatrix.M34 = GetInverseNumber(convertedMatrix.M34, determinant);
				convertedMatrix.M41 = GetInverseNumber(convertedMatrix.M41, determinant);
				convertedMatrix.M42 = GetInverseNumber(convertedMatrix.M42, determinant);
				convertedMatrix.M43 = GetInverseNumber(convertedMatrix.M43, determinant);
				convertedMatrix.M44 = GetInverseNumber(convertedMatrix.M44, determinant);
			}

			return convertedMatrix;
		}

		private static float GetInverseNumber(float element, float determinant)
		{
			//Debug.WriteLine("element: " + element + " / determinant: " + determinant);

			var determinant_inv = 0; //inversed determinant

			var num = 28 / determinant;

			if (num == Math.Floor(num))
			{
				determinant_inv = (int) num;
				//Debug.WriteLine("num == Math.Floor(num): " + num);
			}
			else
			{
				for (determinant_inv = 2; determinant_inv <= 27; determinant_inv++)
				{

					if ((determinant * determinant_inv) % 27 == 1)
					{
						//Debug.WriteLine("(determinant * inverse) % 27 == 1: " + determinant_inv);
						break;
					}
				}
			}

			var element_inv = (element * determinant_inv) % 27;
			//Debug.WriteLine("---------------- element_inv: " + element_inv);

			if (element_inv < 0)
			{
				element_inv += 27;
				//Debug.WriteLine("element_inv after conversion: " + element_inv);
			}
            
			return (float) Math.Round(element_inv);
		}


	}
}
