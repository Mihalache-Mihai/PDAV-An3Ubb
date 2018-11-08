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
        string P3,someDescription,dimension,twoFiveFive;
        List<Block> block8Y = new List<Block>();
        List<Block> blocks8U = new List<Block>();
        List<Block> blocks8V = new List<Block>();

        List<Block> blocks4U = new List<Block>();
        List<Block> blocks4V = new List<Block>();


        List<Block> blocks8UUpSampled = new List<Block>();
        List<Block> blocks8VUpSampled = new List<Block>();


        Pixel[,] finalMatrix = new Pixel[600,800];

        public ReadFromFile()
        {
            this.matrixY = new double[600, 800];
            this.matrixU = new double[600, 800];
            this.matrixV = new double[600, 800];
           
            PutInMatrices();
            createTheBlocks();
            SubSampling();
            DownSampling();
            Decoder();
        }


        public string ReadingFromFile(String filename)
        {
            string text = System.IO.File.ReadAllText(filename);



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

            for (int i = 0; i < lines.Length - 4; i++)
            {
                lines[i] = lines[i + 4];
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
                    
                    altIndiceI = 0;
                    Block y = new Block(8, i, j);
                    Block u = new Block(8, i, j);
                    Block v = new Block(8, i, j);
                    for(int k = i; k < i + 8; k++)
                    {
                        altIndiceJ = 0;
                        for (int l = j; l < j + 8; l++)
                        {
                            y.matrix[altIndiceI, altIndiceJ] = matrixY[k, l];
                            u.matrix[altIndiceI, altIndiceJ] = matrixU[k, l];
                            v.matrix[altIndiceI, altIndiceJ] = matrixV[k, l];
                            altIndiceJ++;
                        }
                        altIndiceI++;
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
                indexI = 0;
                Block u = new Block(4,b.indexi,b.indexj);
                for(int i = 0; i < 8; i+=2)
                {
                    indexJ = 0;
                    for(int j = 0; j < 8; j+=2)
                    {
                        u.matrix[indexI, indexJ] = (b.matrix[i, j] + b.matrix[i, j + 1] + b.matrix[i + 1, j] + b.matrix[i + 1, j + 1]) / 4.00;
                        indexJ++;
                    }
                    indexI++;
                }
                blocks4U.Add(u);
            }

            indexI = 0; indexJ = 0;
            foreach (Block b in blocks8V)
            {
                indexI = 0;
                Block v = new Block(4,b.indexi,b.indexj);
                for (int i = 0; i < 8; i += 2)
                {
                    indexJ = 0;
                    for (int j = 0; j < 8; j += 2)
                    {
                        v.matrix[indexI, indexJ] = (b.matrix[i, j] + b.matrix[i, j + 1] + b.matrix[i + 1, j] + b.matrix[i + 1, j + 1]) / 4.00;
                        indexJ++;
                    }
                    indexI++;
                }
                blocks4V.Add(v);
            }
        }

        public void DownSampling()
        {
            foreach(Block b in blocks4U)
            {
                Block u = new Block(8,b.indexi,b.indexj);
                for (int i = 0; i < 4; i ++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        u.matrix[i * 2, j * 2] = b.matrix[i, j];
                        u.matrix[i * 2 + 1, j * 2] = b.matrix[i, j];
                        u.matrix[i * 2, j * 2 + 1] = b.matrix[i, j];
                        u.matrix[i * 2 + 1, j * 2 + 1] = b.matrix[i, j];
                    }
                }
                blocks8UUpSampled.Add(u);
            }
            foreach (Block b in blocks4V)
            {
                Block v = new Block(8,b.indexi,b.indexj);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        v.matrix[i*2, j*2] = b.matrix[i, j];
                        v.matrix[i*2 , j*2+1] = b.matrix[i, j];
                        v.matrix[i*2+1, j*2] = b.matrix[i, j];
                        v.matrix[i*2 + 1, j*2 + 1] = b.matrix[i, j];
                    }
                }
                blocks8VUpSampled.Add(v);
            }
        }


        public int Clamp(double value)
        {
            if (value > 255)
                return 255;
            else if (value < 0)
            {
                return 0;
            }
            else
                return (int)value;
        }

        public void Decoder()
        {
            for(int i = 0; i < block8Y.Count; i++)
            {
                int startI = block8Y[i].indexi;
                int startJ = block8Y[i].indexj;
                for(int mi = 0; mi < 8; mi++)
                {
                    for(int mj =0; mj < 8; mj++)
                    {
                        double c = block8Y[i].matrix[mi, mj] - 16;
                        double d = blocks8UUpSampled[i].matrix[mi, mj] - 128;
                        double e = blocks8VUpSampled[i].matrix[mi, mj] - 128;
                        int r = Clamp((int)(298 * c + 409 * e + 128) >> 8);
                        int g = Clamp((int)(298 * c - 100 * d - 208 * e + 128) >> 8);
                        int b = Clamp((int)(298 * c + 516 * d + 128) >> 8);

                        finalMatrix[startI + mi, startJ + mj] = new Pixel(r, g, b);
                    }
                }

            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(P3);
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(someDescription);
            stringBuilder.Append("\n");
            stringBuilder.Append(dimension);
            stringBuilder.Append("\n");
            stringBuilder.Append(twoFiveFive);
            foreach (Pixel p in finalMatrix)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append(p.R);
                stringBuilder.Append("\n");
                stringBuilder.Append(p.G);
                stringBuilder.Append("\n");
                stringBuilder.Append(p.B);
            }

            System.IO.File.WriteAllText("C:\\Users\\mihalami\\Desktop\\SecPic.ppm", stringBuilder.ToString());
        }

    }
}
