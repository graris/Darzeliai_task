using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darzeliai
{
	class WriterToFile
	{
		public WriterToFile() { }

		public void WriteListToFile(string filePath, List<Hashtable> darzeliaiData)
		{
			string path = @filePath;

			if (File.Exists(path))
			{
				using (var tw = new StreamWriter(path, true))
				{

					foreach (string key in darzeliaiData.ElementAt(0).Keys)
					{
						tw.Write(key + "; ");
					}

					tw.WriteLine();

					foreach (Hashtable darzelisLineData in darzeliaiData)
					{

						foreach (string key in darzelisLineData.Keys)
						{
							tw.Write(darzelisLineData[key] + "; ");

						}
						tw.WriteLine();
					}
				}
			}


		}

		public void WriteListToFile(string filePath, List<string> darzeliaiData)
		{
			string path = @filePath;

			if (File.Exists(path))
			{
				using (var tw = new StreamWriter(path, true))
				{
					foreach (string darzelisLineData in darzeliaiData)
					{
						tw.WriteLine(darzelisLineData);
					}
				}
			}


		}
		public void WriteLineToFile(string filePath, string lineText)
		{
			string path = @filePath;

			if (File.Exists(path))
			{
				using (var tw = new StreamWriter(path, true))
				{
					tw.WriteLine(lineText);
				}
			}
		}


	}
}
