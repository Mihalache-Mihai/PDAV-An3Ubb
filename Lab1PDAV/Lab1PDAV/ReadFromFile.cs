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
        int[,] quantizationMatrix;
        private String filename = "C:\\Users\\Mihai\\Desktop\\nt-P3.ppm";
        private String filename2 = "C:\\Users\\Mihai\\Desktop\\Lab1PDAV\\Lab1PDAV\\quantizationMatrix.txt";
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

        List<Block> DCT8BlockY = new List<Block>();
        List<Block> DCT8BlockU = new List<Block>();
        List<Block> DCT8BlockV = new List<Block>();

        List<Block> QuantizationBlocks8Y = new List<Block>();
        List<Block> QuantizationBlocks8U = new List<Block>();
        List<Block> QuantizationBlocks8V = new List<Block>();

        List<Block> DeQuantizedBlocks8Y = new List<Block>();
        List<Block> DeQuantizedBlocks8U = new List<Block>();
        List<Block> DeQuantizedBlocks8V = new List<Block>();

        List<double[]> vectorsY = new List<double[]>();
        List<double[]> vectorsU = new List<double[]>();
        List<double[]> vectorsV = new List<double[]>();
        List<int> myIntList = new List<int>();


        Pixel[,] finalMatrix = new Pixel[600,800];

        public static double[] FromZigZagToArray(double[,] matrix)
        {
            double[] result = new double[8 * 8];

            int i = 1;
            int j = 1;

            for (int k = 0; k < 64; k++)
            {
                result[k] = matrix[i - 1, j - 1];
                if ((i + j) % 2 == 0)
                {
                    if (j < 8)
                    {
                        j++;
                    }
                    else
                    {
                        i = i + 2;
                    }
                    if (i > 1)
                    {
                        i--;
                    }
                }
                else
                {
                    if (i < 8)
                    {
                        i++;
                    }
                    else
                    {
                        j = j + 2;
                    }
                    if (j > 1)
                    {
                        j--;
                    }
                }
            }
            return result;
        }

        public void ZigZagParsing()
        {
            foreach (Block b in DeQuantizedBlocks8Y)
            {
                double[] vectorFromY = new double[64];
                vectorFromY = FromZigZagToArray(b.matrix);
                vectorsY.Add(vectorFromY);
            }
            foreach (Block b in DeQuantizedBlocks8U)
            {
                double[] vectorFromU = new double[64];
                vectorFromU = FromZigZagToArray(b.matrix);
                vectorsU.Add(vectorFromU);
            }
            foreach (Block b in DeQuantizedBlocks8V)
            {
                double[] vectorFromV = new double[64];
                vectorFromV = FromZigZagToArray(b.matrix);
                vectorsV.Add(vectorFromV);
            }

        }
        
        public void ThirdEncoder()
        {
            for(int i = 0; i < 7500; i++)
            {
                int zeros = 0;
                double[] arrayY = vectorsY[i];
                double[] arrayU = vectorsU[i];
                double[] arrayV = vectorsV[i];
                int sizeY = GetAmplitude((int)arrayY[0]);
                int sizeU = GetAmplitude((int)arrayU[0]);
                int sizeV = GetAmplitude((int)arrayV[0]);

                myIntList.Add(sizeY);
                myIntList.Add((int)arrayY[0]);
                int jY;
                for (jY = 1; jY < arrayY.Count(); jY++)
                {
                    int auxY = (int)arrayY[jY];
                    if (auxY == 0)
                    {
                        zeros++;
                    }
                    else
                    {
                        sizeY = GetAmplitude(auxY);
                        myIntList.Add(zeros);
                        myIntList.Add(sizeY);
                        myIntList.Add(auxY);
                        zeros = 0;
                    }
                }
                if(zeros > 0)
                {
                    myIntList.Add(0);
                    myIntList.Add(0);
                }

                myIntList.Add(sizeU);
                myIntList.Add((int)arrayU[0]);
                zeros = 0;
                int jU;
                for (jU = 1; jU < arrayU.Count(); jU++)
                {
                    int auxU = (int)arrayU[jU];
                    if(auxU == 0)
                    {
                        zeros++;
                    }
                    else
                    {
                        sizeU = GetAmplitude(auxU);
                        myIntList.Add(zeros);
                        myIntList.Add(sizeU);
                        myIntList.Add(auxU);
                        zeros = 0;
                    }
                }
                if (zeros > 0)
                {
                    myIntList.Add(0);
                    myIntList.Add(0);
                }


                myIntList.Add(sizeV);
                myIntList.Add((int)arrayV[0]);
                zeros = 0;
                int jV;
                for (jV = 1; jV < arrayV.Count(); jV++)
                {
                    int auxV = (int)arrayV[jV];
                    if (auxV == 0)
                    {
                        zeros++;
                    }
                    else
                    {
                        sizeU = GetAmplitude(auxV);
                        myIntList.Add(zeros);
                        myIntList.Add(sizeV);
                        myIntList.Add(auxV);
                        zeros = 0;
                    }
                }
                if (zeros > 0)
                {
                    myIntList.Add(0);
                    myIntList.Add(0);
                }
            }
        }

        public static int GetAmplitude(double value)
        {
            if (value >= -1 && value <= 1)
                return 1;
            if (value >= -3 && value <= -2 || value >= 2 && value <= 3)
                return 2;
            if (value >= -7 && value <= -4 || value >= 4 && value <= 7)
                return 3;
            if (value >= -15 && value <= -8 || value >= 8 && value <= 15)
                return 4;
            if (value >= -31 && value <= -16 || value >= 16 && value <= 31)
                return 5;
            if (value >= -63 && value <= -32 || value >= 32 && value <= 63)
                return 6;
            if (value >= -127 && value <= -64 || value >= 64 && value <= 127)
                return 7;
            if (value >= -255 && value <= -128 || value >= 128 && value <= 255)
                return 8;
            if (value >= -511 && value <= -256 || value >= 256 && value <= 511)
                return 9;
            if (value >= -1023 && value <= -512 || value >= 512 && value <= 1023)
                return 10;

            return 0;
        }

        public ReadFromFile()
        {
            this.matrixY = new double[600, 800];
            this.matrixU = new double[600, 800];
            this.matrixV = new double[600, 800];
            this.quantizationMatrix = new int[8, 8];

            Encoder();
            ForwardDCT();
            Quantization();
            DeQuantization();
            inverseDCT();
            add128();
            ZigZagParsing();
            Decoder();
            ThirdEncoder();


        }
        public void Encoder()
        {
            PutInMatrices();
            createTheBlocks();
            SubSampling();
            UpSampling();
        }

        public void Decoder()
        {
            ConstructFinalMatrix();
        }


        public string ReadingFromFile(String filename)
        {
            string text = System.IO.File.ReadAllText(filename);
            return text;
        }

        public void PutInMatrices()
        {
            string text2 = ReadingFromFile(filename2);
            string[] lines2 = text2.Split(new[] { ' ' }, StringSplitOptions.None);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    quantizationMatrix[i, j] = Int32.Parse(lines2[i * 8 + j]);
                }
            }

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
            int altIndiceI = 0, altIndiceJ = 0;
            for (int i = 0; i < 600; i += 8)
            {
                for (int j = 0; j < 800; j += 8)
                {

                    altIndiceI = 0;
                    Block y = new Block(8, i, j);
                    Block u = new Block(8, i, j);
                    Block v = new Block(8, i, j);
                    for (int k = i; k < i + 8; k++)
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
            foreach (Block b in blocks8U)
            {
                indexI = 0;
                Block u = new Block(4, b.indexi, b.indexj);
                for (int i = 0; i < 8; i += 2)
                {
                    indexJ = 0;
                    for (int j = 0; j < 8; j += 2)
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
                Block v = new Block(4, b.indexi, b.indexj);
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

        public void UpSampling()
        {
            foreach (Block b in blocks4U)
            {
                Block u = new Block(8, b.indexi, b.indexj);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
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
                Block v = new Block(8, b.indexi, b.indexj);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        v.matrix[i * 2, j * 2] = b.matrix[i, j];
                        v.matrix[i * 2, j * 2 + 1] = b.matrix[i, j];
                        v.matrix[i * 2 + 1, j * 2] = b.matrix[i, j];
                        v.matrix[i * 2 + 1, j * 2 + 1] = b.matrix[i, j];
                    }
                }
                blocks8VUpSampled.Add(v);
            }
        }



        public void substract128()
        {          
            //Already made 8x8 matrices in UpSampling() and now block8Y,  blocks8UUpSampled, blocks8VUpSampled are 8x8 matrices UpSampled.
            foreach (Block y in block8Y)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        y.matrix[i, j] -= 128;
                    }
                }
            }
            foreach (Block u in blocks8UUpSampled)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        u.matrix[i, j] -= 128;
                    }
                }
            }

            foreach (Block v in blocks8VUpSampled)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        v.matrix[i, j] -= 128;
                    }
                }
            }

        }

        public void add128()
        {
            //Already made 8x8 matrices in UpSampling() and now block8Y,  blocks8UUpSampled, blocks8VUpSampled are 8x8 matrices UpSampled.
            foreach (Block y in block8Y)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        y.matrix[i, j] += 128;
                    }
                }
            }
            foreach (Block u in blocks8UUpSampled)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        u.matrix[i, j] += 128;
                    }
                }
            }

            foreach (Block v in blocks8VUpSampled)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        v.matrix[i, j] += 128;
                    }
                }
            }

        }

        public void ForwardDCT()
        {
            double oneOver4 = .25;
            double alphaU=0.0, alphaV = 0.0;
            substract128();
            //DCT for 8x8 Y Blocks
            foreach (Block b in block8Y)
            {
                Block result = new Block(8,b.indexi,b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        if (u == 0)
                        {
                            alphaU = 1 / Math.Sqrt(2);
                        }
                        if(u>0)
                        {
                            alphaU = 1;
                        }
                        if (v == 0)
                        {
                            alphaV = 1 / Math.Sqrt(2);
                        }
                        if(v>0)
                        {
                            alphaV = 1;
                        }
                        double sum = 0;
                        result.matrix[u, v] = oneOver4 * alphaU * alphaV;
                        for (int x = 0; x < 8; x++)
                        {
                            for(int y = 0; y < 8; y++)
                            {
                      
                                sum += b.matrix[x, y] * (Math.Cos(((2 * x + 1) * u * Math.PI) / 16) * Math.Cos(((2 * y + 1) * v * Math.PI) / 16));

                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                DCT8BlockY.Add(result);

            }
            //DCT for 8x8 U blocks
            foreach (Block b in blocks8UUpSampled)
            {
                Block result = new Block(8, b.indexi, b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        if (u == 0)
                        {
                            alphaU = 1 / Math.Sqrt(2);
                        }
                        if(u>0)
                        {
                            alphaU = 1;
                        }
                        if (v == 0)
                        {
                            alphaV = 1 / Math.Sqrt(2);
                        }
                        if(v>0)
                        {
                            alphaV = 1;
                        }
                        double sum = 0;
                        result.matrix[u, v] = oneOver4 * alphaU * alphaV;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {

                                sum += b.matrix[x, y] * Math.Cos(((2 * x + 1) * u * Math.PI) / 16) * Math.Cos(((2 * y + 1) * v * Math.PI) / 16);

                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                DCT8BlockU.Add(result);
            }

            //DCT for 8x8 V blocks
            foreach (Block b in blocks8VUpSampled)
            {
                Block result = new Block(8, b.indexi, b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        if (u == 0)
                        {
                            alphaU = 1 / Math.Sqrt(2);
                        }
                        if(u>0)
                        {
                            alphaU = 1;
                        }
                        if (v == 0)
                        {
                            alphaV = 1 / Math.Sqrt(2);
                        }
                        if(v>0)
                        {
                            alphaV = 1;
                        }
                        double sum = 0;
                        result.matrix[u, v] = oneOver4 * alphaU * alphaV;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {

                                sum += b.matrix[x, y] * Math.Cos(((2 * x + 1) * u * Math.PI) / 16) * Math.Cos(((2 * y + 1) * v * Math.PI) / 16);

                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                DCT8BlockV.Add(result);
            }

        }

        public void inverseDCT()
        {
            block8Y = new List<Block>();
            blocks8UUpSampled = new List<Block>();
            blocks8VUpSampled = new List<Block>();

            double oneOver4 = .25;
            double alphaX, alphaY;
            //Inverse DCT for 8x8 Y Blocks
            foreach (Block b in DeQuantizedBlocks8Y)
            {
                Block result = new Block(8, b.indexi, b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        double sum = 0;
                        result.matrix[u, v] = oneOver4;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                if (x == 0)
                                {
                                    alphaX = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaX = 1;
                                }
                                if (y == 0)
                                {
                                    alphaY = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaY = 1;
                                }

                                sum +=alphaX* alphaY* b.matrix[x, y] * Math.Cos(((2 * u + 1) * x * Math.PI) / 16) * Math.Cos(((2 * v + 1) * y * Math.PI) / 16);

                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                block8Y.Add(result);

            }
            foreach (Block b in DeQuantizedBlocks8U)
            {
                Block result = new Block(8, b.indexi, b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        
                        double sum = 0;
                        result.matrix[u, v] = oneOver4;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                if (x == 0)
                                {
                                    alphaX = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaX = 1;
                                }
                                if (y == 0)
                                {
                                    alphaY = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaY = 1;
                                }

                                sum += alphaX * alphaY * b.matrix[x, y] * Math.Cos(((2 * u + 1) * x * Math.PI) / 16) * Math.Cos(((2 * v + 1) * y * Math.PI) / 16);
                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                blocks8UUpSampled.Add(result);

            }

            foreach (Block b in DeQuantizedBlocks8V)
            {
                Block result = new Block(8, b.indexi, b.indexj);
                for (int u = 0; u < 8; u++)
                {
                    for (int v = 0; v < 8; v++)
                    {
                        double sum = 0;
                        result.matrix[u, v] = oneOver4;
                        for (int x = 0; x < 8; x++)
                        {
                            for (int y = 0; y < 8; y++)
                            {
                                if (x == 0)
                                {
                                    alphaX = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaX = 1;
                                }
                                if (y == 0)
                                {
                                    alphaY = 1 / Math.Sqrt(2);
                                }
                                else
                                {
                                    alphaY = 1;
                                }

                                sum += alphaX * alphaY * b.matrix[x, y] * Math.Cos(((2 * u + 1) * x * Math.PI) / 16) * Math.Cos(((2 * v + 1) * y * Math.PI) / 16);

                            }
                        }
                        result.matrix[u, v] *= sum;
                    }
                }
                blocks8VUpSampled.Add(result);

            }

        }


        
       
        public void Quantization()
        {
            foreach (Block b in DCT8BlockY)
            {
                Block quantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r =Convert.ToInt32(b.matrix[x, y] / quantizationMatrix[x, y]);
                        quantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                QuantizationBlocks8Y.Add(quantizedResult);
            }

            foreach (Block b in DCT8BlockU)
            {
                Block quantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r = Convert.ToInt32(b.matrix[x, y] / quantizationMatrix[x, y]);
                        quantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                QuantizationBlocks8U.Add(quantizedResult);
            }

            foreach (Block b in DCT8BlockV)
            {
                Block quantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r = Convert.ToInt32(b.matrix[x, y] / quantizationMatrix[x, y]);
                        quantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                QuantizationBlocks8V.Add(quantizedResult);
            }
            
        }

        public void DeQuantization()
        {
            foreach (Block b in QuantizationBlocks8Y)
            {
                Block deQuantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r = Convert.ToInt32(b.matrix[x, y] * quantizationMatrix[x, y]);
                        deQuantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                DeQuantizedBlocks8Y.Add(deQuantizedResult);
            }

            foreach (Block b in QuantizationBlocks8U)
            {
                Block deQuantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r = Convert.ToInt32(b.matrix[x, y] * quantizationMatrix[x, y]);
                        deQuantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                DeQuantizedBlocks8U.Add(deQuantizedResult);
            }

            foreach (Block b in QuantizationBlocks8V)
            {
                Block deQuantizedResult = new Block(8, b.indexi, b.indexj);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        int r = Convert.ToInt32(b.matrix[x, y] * quantizationMatrix[x, y]);
                        deQuantizedResult.matrix[x, y] = Convert.ToDouble(r);
                    }
                }
                DeQuantizedBlocks8V.Add(deQuantizedResult);
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

        public void ConstructFinalMatrix()
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

            System.IO.File.WriteAllText("C:\\Users\\Mihai\\Desktop\\SecPic.ppm", stringBuilder.ToString());
        }

    }
}
