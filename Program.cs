using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMI_FMU_Plugin
{
    class FMI_FMU_Plugin
    {
        static void Main(string[] args)
        {
            Evaluate();
        }

        static double rotationVel = 18.13968781;
        static double airSpeed = 66.3592933;
        static double altitude = 3560.898148;

        static List<double> Evaluate()
        {
            string fileName = (@"C:\Users\vlesser\Desktop\OutputTable.csv");

            //Create lists of column values
            List<string> name = new List<string>();
            List<double> columnP6 = new List<double>();
            List<double> columnP10 = new List<double>();
            List<double> columnP12 = new List<double>();

            List<double> columnP1 = new List<double>();
            List<double> columnP2 = new List<double>();
            List<double> columnP3 = new List<double>();
            List<double> columnp7 = new List<double>();
            List<double> columnP8 = new List<double>();
            List<double> columnP9 = new List<double>();

            List<double> returnValues = new List<double>();

            String inputString = File.ReadAllText(fileName);
            //Read all rows and save as list 'rowsCSV'
            List<string> rowsCSV = File.ReadAllLines(fileName).ToList();

            foreach (string wholeLine in rowsCSV)
            {
                //separate values by comma and save as array
                string[] values = wholeLine.Split(',');

                columnP6.Add(Convert.ToDouble(values[1]));
                columnP10.Add(Convert.ToDouble(values[2]));
                columnP12.Add(Convert.ToDouble(values[3]));

                columnP1.Add(Convert.ToDouble(values[4]));
                columnP2.Add(Convert.ToDouble(values[5]));
                columnP3.Add(Convert.ToDouble(values[6]));
                columnp7.Add(Convert.ToDouble(values[7]));
                columnP8.Add(Convert.ToDouble(values[8]));
                columnP9.Add(Convert.ToDouble(values[9]));

            }


            double closestnumberForP6 = columnP6.OrderBy(x => Math.Abs(rotationVel - x)).First();
            double closestnumberForP10 = columnP10.OrderBy(x => Math.Abs(airSpeed - x)).First();
            double closestnumberForP12 = columnP12.OrderBy(x => Math.Abs(altitude - x)).First();

            Console.WriteLine("Closest numbers are: " + closestnumberForP6 + ", " + closestnumberForP10 + ", " + closestnumberForP12);

            int indexOfMatchingElement = 0;

            if (isOneElementinList(columnP6, closestnumberForP6))
            {
                indexOfMatchingElement = columnP6.IndexOf(closestnumberForP6);
            }
            else if (isOneElementinList(columnP10, closestnumberForP10))
            {
                indexOfMatchingElement = columnP10.IndexOf(closestnumberForP10);
            }
            else if (matchingIndexOfTwoLists(columnP6, columnP10, closestnumberForP6, closestnumberForP10).Count == 1)
            {
                indexOfMatchingElement = matchingIndexOfTwoLists(columnP6, columnP10, closestnumberForP6, closestnumberForP10)[0];
            }
            else if (isOneElementinList(columnP12, closestnumberForP12))
            {
                indexOfMatchingElement = columnP12.IndexOf(closestnumberForP12);
            }
            else
            {
                indexOfMatchingElement = matchingIndexFromThreeLists(columnP6, columnP10, columnP12, closestnumberForP6, closestnumberForP10, closestnumberForP12);
            }

            returnValues.AddRange(new List<double>() { columnP1[indexOfMatchingElement], columnP2[indexOfMatchingElement],
                columnP3[indexOfMatchingElement], columnp7[indexOfMatchingElement],
                columnP8[indexOfMatchingElement], columnP9[indexOfMatchingElement] });

            for (int i = 0; i < returnValues.Count; i++)
            {
                Console.WriteLine("Value is: " + returnValues[i]);
            }

            Console.ReadLine();

            return returnValues;

        }


        static Boolean isOneElementinList(List<double> values, double targetnumber)
        {
            int duplicateCount = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] == targetnumber)
                {
                    duplicateCount++;
                }
            }

            if (duplicateCount <= 1)
            {
                return true;
            }

            return false;
        }



        static List<int> matchingIndexOfTwoLists(List<double> firstListOfValues, List<double> secondListOfvalues, double firstTargetnumber, double secondTargetNumbers)
        {
            List<int> indexOfFirstValueDuplicates = new List<int>();
            List<int> indexOfSecondValueDuplicates = new List<int>();


            for (int i = 0; i < firstListOfValues.Count; i++)
            {
                if (firstListOfValues[i] == firstTargetnumber)
                {
                    indexOfFirstValueDuplicates.Add(i);
                }
            }

            for (int i = 0; i < secondListOfvalues.Count; i++)
            {
                if (secondListOfvalues[i] == secondTargetNumbers)
                {
                    indexOfSecondValueDuplicates.Add(i);
                }
            }

            List<int> commonIndexes = indexOfFirstValueDuplicates.Intersect(indexOfSecondValueDuplicates).ToList<int>();


            return commonIndexes;
        }



        static int matchingIndexFromThreeLists(List<double> firstListOfValues, List<double> secondListOfvalues, List<double> thirdListOfvalues
            , double firstTargetnumber, double secondTargetNumbers, double thirdTargetNumber)
        {

            List<int> commonIndexesOfTwoLists = matchingIndexOfTwoLists(firstListOfValues, secondListOfvalues, firstTargetnumber, secondTargetNumbers);
            List<int> thirdListDupllicatesList = new List<int>();

            for (int i = 0; i < thirdListOfvalues.Count; i++)
            {
                if (thirdListOfvalues[i] == thirdTargetNumber)
                {
                    thirdListDupllicatesList.Add(i);
                }
            }

            return commonIndexesOfTwoLists.Intersect(thirdListDupllicatesList).ToList<int>()[0];

        }
    }
}
