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
        private String filename = "C:\\Users\\Mihai\\Desktop\\nt-P3.ppm";
        string line;
        int numberOfLines;
        string P3,someDescription,dimension,twoFiveFive;
        List<Block> block8Y = new List<Block>();
        List<Block> blocks8U = new List<Block>();
        List<Block> blocks8V = new List<Block>();

        List<Block> blocks4U = new List<Block>();
        List<Block> blocks4V = new List<Block>();

        public ReadFromFile()
        {
            this.matrixY = new double[600, 800];
            this.matrixU = new double[600, 800];
            this.matrixV = new double[600, 800];
           
            PutInMatrices();
            createTheBlocks();
            SubSampling();
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

            P3 = lines[0];
            someDescription = lines[1];
            dimension = lines[2];
            twoFiveFive = lines[3];
            string[] firstLines = { P3, someDescription, dimension, twoFiveFive };
            StringBuilder stringBuilder = new StringBuilder();

            //foreach (string s in lines)
            //{
            //    stringBuilder.Append(s);
            //    stringBuilder.Append('\n');
            //}
            //System.IO.File.WriteAllText("C:\\Users\\Mihai\\Desktop\\WriteLines.ppm", text);
            for (int i = 0; i < lines.Length - 4; i++)
            {
                lines[i] = lines[i + 4];
                //Console.WriteLine(lines[i]);
            }
            this.numberOfLines = lines.Length - 3;

            int currentNumber = 0;
            for (int i = 0; i < 600; i++)
            {
                for (int j = 0; j < 800; j++)
                {
                    matrixY[i, j] = 0.299 * Int32.Parse(lines[currentNumber]) + 0.587 * Int32.Parse(lines[currentNumber + 1]) + 0.144 * Int32.Parse(lines[currentNumber + 2]);
                    matrixU[i, j] = 128 - 0.1687 * Int32.Parse(lines[currentNumber]) - 0.3312 * Int32.Parse(lines[currentNumber + 1]) + 0.5 * Int32.Parse(lines[currentNumber + 2]);
                    matrixV[i, j] = 128 + 0.5 * Int32.Parse(lines[currentNumber]) - 0.4186 * Int32.Parse(lines[currentNumber + 1]) - 0.0813 * Int32.Parse(lines[currentNumber + 2]);
                    currentNumber += 3;
                }
            }
            Console.WriteLine("asd");

        }

        public void createTheBlocks()
        {
            int altIndiceI=0, altIndiceJ=0;
            for(int i = 0; i < 600; i += 8)
            {
                for(int j = 0; j < 800; j += 8)
                {
                    
                    altIndiceJ = 0;
                    Block y = new Block(8, i, j);
                    Block u = new Block(8, i, j);
                    Block v = new Block(8, i, j);
                    for(int k = i; k < i + 8; k++)
                    {
                        altIndiceI = 0;
                        for (int l = j; l < j + 8; l++)
                        {
                            y.matrix[altIndiceI, altIndiceJ] = matrixY[k, l];
                            u.matrix[altIndiceI, altIndiceJ] = matrixU[k, l];
                            v.matrix[altIndiceI, altIndiceJ] = matrixV[k, l];
                            altIndiceI++;
                        }
                        altIndiceJ++;
                    }
                    this.block8Y.Add(y);
                    this.blocks8U.Add(u);
                    this.blocks8V.Add(v);
                }
            }
        }
        public void SubSampling()
        {
            int indexI = 0, indexJ = 0;
            foreach(Block b in blocks8U)
            {
                indexJ = 0;
                Block u = new Block(4);
                for(int i = 0; i < 8; i+=2)
                {
                    indexI = 0;
                    for(int j = 0; j < 8; j+=2)
                    {
                        u.matrix[indexI, indexJ] = (b.matrix[i, j] + b.matrix[i, j + 1] + b.matrix[i + 1, j] + b.matrix[i + 1, j + 1]) / 4;
                        indexI++;
                    }
                    indexJ++;
                }
                blocks4U.Add(u);
            }

            indexI = 0; indexJ = 0;
            foreach (Block b in blocks8V)
            {
                indexJ = 0;
                Block v = new Block(4);
                for (int i = 0; i < 8; i += 2)
                {
                    indexI = 0;
                    for (int j = 0; j < 8; j += 2)
                    {
                        v.matrix[indexI, indexJ] = (b.matrix[i, j] + b.matrix[i, j + 1] + b.matrix[i + 1, j] + b.matrix[i + 1, j + 1]) / 4;
                        indexI++;
                    }
                    indexJ++;
                }
                blocks4V.Add(v);
            }
        }
    }
}
