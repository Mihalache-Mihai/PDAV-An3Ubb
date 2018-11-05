using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1PDAV
{
    class ReadFromFile
    {
        double[,] matrixY, matrixU, matrixV;
        private String filename = "C:\\Users\\mihalami\\Desktop\\nt-P3.ppm";
        string line;
        int numberOfLines;
        public ReadFromFile()
        {
            this.matrixY = new double[600, 800];
            this.matrixU = new double[600, 800];
            this.matrixV = new double[600, 800];
            PutInMatrices();
        }


        public string ReadingFromFile(String filename)
        {
            string text = System.IO.File.ReadAllText(filename);

            // Display the file contents to the console. Variable text is a string.
            //System.Console.WriteLine("Contents of photo" + text);

            return text;
        }

        public void PutInMatrices()
        {
            string text = ReadingFromFile(filename);
            string[] lines = text.Split(new[] { '\n' }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length-3; i++)
            {
                lines[i] = lines[i + 3];
                //Console.WriteLine(lines[i]);
            }
            this.numberOfLines = lines.Length - 3;

            int currentNumber = 0;
            for(int i = 0; i < 600; i++)
            {
                for(int j = 0; j < 800; j ++)
                {
                    matrixY[i,j] = 0.299 * Int32.Parse(lines[currentNumber]) + 0.587 * Int32.Parse(lines[currentNumber + 1]) + 0.144 * Int32.Parse(lines[currentNumber + 2]);
                    matrixU[i,j] = 128 - 0.1687 * Int32.Parse(lines[currentNumber]) - 0.3312 * Int32.Parse(lines[currentNumber + 1]) + 0.5 * Int32.Parse(lines[currentNumber + 2]);
                    matrixV[i,j] = 128 + 0.5 * Int32.Parse(lines[currentNumber]) - 0.4186 * Int32.Parse(lines[currentNumber + 1]) - 0.0813 * Int32.Parse(lines[currentNumber + 2]);
                    currentNumber += 3;
                }
            }
            Console.WriteLine("asd");
        }
    }
}
