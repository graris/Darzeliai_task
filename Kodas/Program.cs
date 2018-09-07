using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Darzeliai
{
	// ##########################
	// Made by Dominykas Stakutis
	// ##########################
	
	class Program
	{
		private static List<Hashtable> darzeliaiData = new List<Hashtable>();

		private static WriterToFile wtf = new WriterToFile();

		private static String[] filePaths = {
			"Rezultatai/1_iAtmintiNuskaitiDuomenysIsCSV.txt",
			"Rezultatai/2_MAX_MIN_CHILDS_COUNT.txt",
			"Rezultatai/3_eiluciuZodziai_Kur_MAX_MIN_CHILDS_COUNT.txt",
			"Rezultatai/4_darzeliaiPagalKalbaKurieTuriDaugiausiaiLaisvuVietuProcentais.txt",
			"Rezultatai/5_darzeliaiKuriuoseYra2-4LaisvuViet_Sorted_Z-A.txt"
		};

		static void Main(string[] args)
		{

			CSVreader reader = new CSVreader("Darzeliu galimu priimti ir lankantys vaikai2018.csv");

			darzeliaiData = reader.readCSVfileContentToList();

			OutputImportedDarzeliaiData();

			//--------------------------------------------------------------------------------------------------------------------------------

			DarzeliaiDataExplorer darzDataExplorer = new DarzeliaiDataExplorer(darzeliaiData);

			int minChildsCount = darzDataExplorer.FindMinChildCount(),
				maxChildsCount = darzDataExplorer.FindMaxChildCount();

			OutputChildsMinMaxCountsInfo(minChildsCount, maxChildsCount);

			//--------------------------------------------------------------------------------------------------------------------------------


			DarzeliaiDataWordConstructor darzDataWordConstructor = new DarzeliaiDataWordConstructor(darzeliaiData);

			List<string> darzeliaiWordsListOfMaxChildCount = darzDataWordConstructor.constructWordsListFromDarzeliaiLinesData(darzDataExplorer.FindLinesDataByChildCountValue(maxChildsCount));
			List<string> darzeliaiWordsListOfMinChildCount = darzDataWordConstructor.constructWordsListFromDarzeliaiLinesData(darzDataExplorer.FindLinesDataByChildCountValue(minChildsCount));

			OutputDarzeliaiListsOfMaxMinChildsCounts(darzeliaiWordsListOfMaxChildCount, darzeliaiWordsListOfMinChildCount);

			//--------------------------------------------------------------------------------------------------------------------------------

			Tuple<string, double> MaxPercentOfFreeSpaceByLanguageGroup = darzDataExplorer.FindMaxPercentOfFreeSpaceByLanguageGroup(darzeliaiData);

			OutputMaxPercentOfFreeSpaceByLanguageInfo(MaxPercentOfFreeSpaceByLanguageGroup);

			//--------------------------------------------------------------------------------------------------------------------------------

			List<Hashtable> groupedDarzeliaiData = new List<Hashtable>();

			groupedDarzeliaiData = darzDataExplorer.GroupDarzeliaiDataByColumn("SCHOOL_NAME", darzDataExplorer.SelectDarzeliaiInFreeSpaceRange(darzeliaiData, 2, 4));//DarzeliaiDataGroupBySchoolName(darzDataExplorer.SelectDarzeliaiInFreeSpaceRange(darzeliaiData, 2, 4));

			OutputGroupedDarzeliaiDataBySchoolName_sortedZ_A(groupedDarzeliaiData);

			Console.ReadKey();
		}

		private static void OutputGroupedDarzeliaiDataBySchoolName_sortedZ_A(List<Hashtable> groupedDarzeliaiData)
		{
			const string headerForData = "\nDarželiai, kuriuose yra nuo 2 iki 4 laisvų vietų (sugrupuoti pagal pavadinimą ir išrūšiuoti nuo Z iki A, vaikų skaičiai ir laisvos vietos susumuotos): ";

			Console.WriteLine(headerForData);

			groupedDarzeliaiData.Sort((Hashtable x, Hashtable y) => y["SCHOOL_NAME"].ToString().Replace("\"", "").CompareTo(x["SCHOOL_NAME"].ToString().Replace("\"", "")));

			foreach (Hashtable darzByLan in groupedDarzeliaiData)
			{
				foreach (string key in darzByLan.Keys)
					Console.Write(darzByLan[key] + "; ");

				Console.WriteLine();
			}

			System.IO.File.WriteAllText(filePaths[4], string.Empty);

			wtf.WriteLineToFile(filePaths[4], headerForData);

			wtf.WriteListToFile(filePaths[4], groupedDarzeliaiData);
		}

		private static void OutputMaxPercentOfFreeSpaceByLanguageInfo(Tuple<string, double> MaxPercentOfFreeSpaceByLanguageGroup)
		{
			const string outputDataFormat = "\n{0} kalbos darželiai turi daugiausiai laisvų vietų procentais: {1:F2}%";

			Console.WriteLine(outputDataFormat, MaxPercentOfFreeSpaceByLanguageGroup.Item1, MaxPercentOfFreeSpaceByLanguageGroup.Item2);

			System.IO.File.WriteAllText(filePaths[3], string.Empty);

			wtf.WriteLineToFile(filePaths[3], string.Format(outputDataFormat, MaxPercentOfFreeSpaceByLanguageGroup.Item1, MaxPercentOfFreeSpaceByLanguageGroup.Item2));
		}

		private static void OutputDarzeliaiListsOfMaxMinChildsCounts(List<string> darzeliaiWordsListOfMaxChildCount, List<string> darzeliaiWordsListOfMinChildCount)
		{
			const string headerForMaxChildsCountData = "\nEiluciu zodziai, kuriose stulpelio „CHILDS_COUNT“ reikšmė yra didžiausia: ";
			const string headerForMinChildsCountData = "\nEiluciu zodziai, kuriose stulpelio „CHILDS_COUNT“ reikšmė yra mažiausia: ";

			Console.WriteLine(headerForMaxChildsCountData);
			foreach (string word in darzeliaiWordsListOfMaxChildCount)
				Console.WriteLine(word);

			Console.WriteLine(headerForMinChildsCountData);
			foreach (string word in darzeliaiWordsListOfMinChildCount)
				Console.WriteLine(word);

			System.IO.File.WriteAllText(filePaths[2], string.Empty);

			wtf.WriteLineToFile(filePaths[2], headerForMaxChildsCountData);
			wtf.WriteListToFile(filePaths[2], darzeliaiWordsListOfMaxChildCount);
			wtf.WriteLineToFile(filePaths[2], headerForMinChildsCountData);
			wtf.WriteListToFile(filePaths[2], darzeliaiWordsListOfMinChildCount);
		}

		private static void OutputChildsMinMaxCountsInfo(int minChildsCount, int maxChildsCount)
		{
			const string maxCountInfoTxt = "Max child count: ";
			const string minCountInfoTxt = "Min child count: ";

			Console.WriteLine(maxCountInfoTxt + maxChildsCount);
			
			Console.WriteLine(minCountInfoTxt + minChildsCount);

			System.IO.File.WriteAllText(filePaths[1], string.Empty);

			wtf.WriteLineToFile(filePaths[1], maxCountInfoTxt + maxChildsCount);
			wtf.WriteLineToFile(filePaths[1], minCountInfoTxt + minChildsCount);
		}

		private static void OutputImportedDarzeliaiData()
		{
			System.IO.File.WriteAllText(filePaths[0], string.Empty);

			wtf.WriteLineToFile(filePaths[0], "Nuskaityti duomenis į atmintį iš .csv tipo failo:");

			wtf.WriteListToFile(filePaths[0], darzeliaiData);
		}
	}
}
