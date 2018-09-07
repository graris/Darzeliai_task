using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Darzeliai 
{
	class CSVreader
	{
		private const char Separator = ';';

		private String filePath = "";

		public CSVreader(String filePath) => this.filePath = filePath;

		public List<Hashtable> readCSVfileContentToList() {

			List<Hashtable> darzeliaiData = new List<Hashtable>();

			StreamReader sr = File.OpenText(path: filePath);

			String[] keys = sr.ReadLine().Split(Separator);

			string line = "";
			string[] lineData = null;

			while ((line = sr.ReadLine()) != null)
			{
				Hashtable newLineData = new Hashtable();

				lineData = line.Split(Separator);

				for (int i = 0; i < lineData.Length; i++)
					newLineData.Add(key: keys[i], value: lineData[i]);

				darzeliaiData.Add(item: newLineData);

			}

			return darzeliaiData;
		}
	}
}
