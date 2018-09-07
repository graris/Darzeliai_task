using System;
using System.Collections;
using System.Collections.Generic;

namespace Darzeliai
{
	class DarzeliaiDataExplorer
	{
		private List<Hashtable> darzeliaiData;

		public DarzeliaiDataExplorer(List<Hashtable> darzeliaiData) => this.darzeliaiData = darzeliaiData;

		public int FindMaxChildCount() {

			int maxChildsCount = int.MinValue;

			foreach (Hashtable darzelis in darzeliaiData)	
				if (int.Parse(darzelis["CHILDS_COUNT"].ToString()) > maxChildsCount)
					maxChildsCount = int.Parse(darzelis["CHILDS_COUNT"].ToString());

			return maxChildsCount;
		}

		public int FindMinChildCount() {

			int minChildsCount = int.MaxValue;

			foreach (Hashtable darzelis in darzeliaiData)
				if (int.Parse(darzelis["CHILDS_COUNT"].ToString()) < minChildsCount)
					minChildsCount = int.Parse(darzelis["CHILDS_COUNT"].ToString());

			return minChildsCount;
		}

		public List<Hashtable> FindLinesDataByChildCountValue(int childCount)
		{
			List<Hashtable> linesData = new List<Hashtable>();

			foreach (Hashtable lineData in darzeliaiData)
				if (int.Parse(lineData["CHILDS_COUNT"].ToString()) == childCount)
					linesData.Add(lineData);

			return linesData;
		}

		public List<Hashtable> GroupDarzeliaiDataByColumn(string columnName, List<Hashtable> darzeliaiData)
		{
			List<Hashtable> groupedDarzeliaiData = new List<Hashtable>();

			Boolean groupedListDoesNotContainThisLan = true;

			foreach (Hashtable lineData in darzeliaiData)
			{
				groupedListDoesNotContainThisLan = true;

				for (int i = 0; i < groupedDarzeliaiData.Count; i++)
				{
					if (lineData[columnName].Equals(groupedDarzeliaiData[i][columnName]))
					{
						int tempSum = 0;
						tempSum = int.Parse(groupedDarzeliaiData[i]["CHILDS_COUNT"].ToString()) + int.Parse(lineData["CHILDS_COUNT"].ToString());
						groupedDarzeliaiData[i]["CHILDS_COUNT"] = tempSum;

						tempSum = 0;
						tempSum = int.Parse(groupedDarzeliaiData[i]["FREE_SPACE"].ToString()) + int.Parse(lineData["FREE_SPACE"].ToString());
						groupedDarzeliaiData[i]["FREE_SPACE"] = tempSum;

						groupedListDoesNotContainThisLan = false;
					}

				}

				if ((groupedDarzeliaiData.Count == 0) || (groupedListDoesNotContainThisLan))
				{
					Hashtable darzeliaiDataGroupByLan = new Hashtable();

					darzeliaiDataGroupByLan.Add(columnName, lineData[columnName]);
					darzeliaiDataGroupByLan.Add("CHILDS_COUNT", lineData["CHILDS_COUNT"]);
					darzeliaiDataGroupByLan.Add("FREE_SPACE", lineData["FREE_SPACE"]);

					groupedDarzeliaiData.Add(darzeliaiDataGroupByLan);

					groupedListDoesNotContainThisLan = true;
				}
			}
			return groupedDarzeliaiData;
		}

		public List<Hashtable> SelectDarzeliaiInFreeSpaceRange(List<Hashtable> darzeliaiData, int from, int to)
		{
			List<Hashtable> selectedDarzeliaiData = new List<Hashtable>();

			foreach (Hashtable darzelisData in darzeliaiData)
				if ((int.Parse(darzelisData["FREE_SPACE"].ToString()) >= from)&&(int.Parse(darzelisData["FREE_SPACE"].ToString()) <= to))
					selectedDarzeliaiData.Add(darzelisData);

			return selectedDarzeliaiData;
		}

		public Tuple<string, double> FindMaxPercentOfFreeSpaceByLanguageGroup(List<Hashtable> darzeliaiData)
		{
			List<Hashtable> darzeliaiDataGroupedByLang = GroupDarzeliaiDataByColumn("LAN_LABEL", darzeliaiData);

			Hashtable darzeliaiPercentOfFreeSpaceByLang = CalculatePercentOfFreeSpaceForEachDarzeliaiLanguageGroup(darzeliaiDataGroupedByLang);

			double maxPercentOfFreeSpace = int.MinValue;
			string languageGroupWithMaxPercent = "";

			foreach (string langGroup in darzeliaiPercentOfFreeSpaceByLang.Keys)
			{
				double percentOfFreeSpace = double.Parse(darzeliaiPercentOfFreeSpaceByLang[langGroup].ToString());

				if (percentOfFreeSpace > maxPercentOfFreeSpace)
				{
					languageGroupWithMaxPercent = langGroup;
					maxPercentOfFreeSpace = percentOfFreeSpace;
				}
			}

			return new Tuple<string, double>(languageGroupWithMaxPercent, maxPercentOfFreeSpace);
		}

		public Hashtable CalculatePercentOfFreeSpaceForEachDarzeliaiLanguageGroup(List<Hashtable> groupedByLangDarzeliaiData)
		{
			Hashtable darzeliaiPercentOfFreeSpaceByLang = new Hashtable();

			foreach (Hashtable oneLangGroup in groupedByLangDarzeliaiData)
			{

				double sumOfAllSpace = double.Parse(oneLangGroup["CHILDS_COUNT"].ToString()) + double.Parse(oneLangGroup["FREE_SPACE"].ToString());
				double percentOfFreeSpace = double.Parse(oneLangGroup["FREE_SPACE"].ToString()) * 100 / sumOfAllSpace;
				darzeliaiPercentOfFreeSpaceByLang.Add(oneLangGroup["LAN_LABEL"], percentOfFreeSpace);

			}

			return darzeliaiPercentOfFreeSpaceByLang;
		}



	}
}
