using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class CarCollision : MonoBehaviour
{
    public ArcadeCar check;//the main object - the car

    /// <summary>
    /// This Function Sums all the fitness scors
    /// </summary>
    /// <param name="fileName"> The file name of the file ahtat all the fitness scores are in</param>
    /// <returns> the sum of the scores</returns>
    double GetFitnessScoreSum(string fileName)
    {
        double SumOfScore = 0;
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Open)))
        {
            for (int j = 0; j < 50; j++)
            {
                br.ReadInt32();
                SumOfScore += br.ReadDouble();
            }
        }
        Debug.Log("Sum Of FitnessScores: " + SumOfScore);
        return SumOfScore;
    }

    /// <summary>
    /// This function calculates the Probability of each thought to be mutate to the next gen
    /// Thought`s Probability = Probability before + (current Fitness / Sum of Futness)
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns> returns an array with all the probebilities for each thought to get picked for next gen
    /// the sum of all probebilities is 1 each Probability is between 0-1</returns>
    double[] GetThoughtsProbability(string fileName)
    {
        double SumOfScore = GetFitnessScoreSum(fileName);
        double[] ThoughtsProbability = new double[50];
        double SumOfProbability = 0.0;
        double currFit;
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Open)))
        {
            for (int j = 0; j < 50; j++)
            {
                br.ReadInt32();
                currFit = br.ReadDouble();
                ThoughtsProbability[j] = SumOfProbability + (currFit / SumOfScore);
                SumOfProbability += ThoughtsProbability[j];
            }
        }
        Debug.Log("Thought Probability done");
        for (int i = 0; i < 50; i++)
        {
            Debug.Log("Prob: " + ThoughtsProbability[i]);
        }
        return ThoughtsProbability;
    }

    /// <summary>
    /// this funciton doing a Roulette for all the thoughts and get out
    /// the thoughts that came out (the Thoughts Probability is based on its fitnessscore)
    /// </summary>
    /// <param name="fileName"> the file to read the thoughts and calculate the probability with
    /// GetThoughtsProbability function</param>
    /// <param name="numberOfThoughts"> the number of wanted thoughts to get</param>
    /// <returns> an array size of numberOfThoughts the array contains the intexes for the pickes thoughts to be mutated</returns>
    int[] ThoughtsRouletteWheelSelection(string fileName, int numberOfThoughts)
    {
        System.Random random = new System.Random();
        double[] ThoughtsProbability = GetThoughtsProbability(fileName);
        int[] ThoughtIndexs = new int[numberOfThoughts];
        for (int i = 0; i < numberOfThoughts; i++)
        {
            double target = random.NextDouble();
            for (int j = 0; j < 50; j++)
            {
                if(j == 49)
                {
                    if (target >= ThoughtsProbability[j])
                    {
                        ThoughtIndexs[i] = j;
                        break;
                    }
                }
                else if(target >= ThoughtsProbability[j] && target < ThoughtsProbability[j+1])
                {
                    ThoughtIndexs[i] = j;
                    break;
                }
            }
        }
        Debug.Log("Roulette has been done");
        return ThoughtIndexs;
    }

    /// <summary>
    /// this funciton return an array of the iindexses of the best thoughts
    /// </summary>
    /// <param name="NumberOfThoughts"> the number of best thoughts index to return</param>
    /// <returns>return an array of the iindexses of the best thoughts </returns>
    double[] GetWeightsIndex(int NumberOfThoughts)
    {
        double[] WeightsIndexs = new double[NumberOfThoughts];
        for (int i = 0; i < NumberOfThoughts; i++)
        {
            WeightsIndexs[i] = -1;
        }
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\FitnessScores.bin", FileMode.Open)))
        {
            for (int i = 0; i < NumberOfThoughts; i++)
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                double maxScore = -1;
                int maxIndex = -1;
                int tempIndex;
                double tempScore;
                for (int j = 0; j < 50; j++)
                {
                    tempIndex = br.ReadInt32();
                    tempScore = br.ReadDouble();
                    if (tempScore > maxScore)
                    {
                        if(Array.IndexOf(WeightsIndexs, (double)tempIndex) < 0)
                        {
                            maxIndex = tempIndex;
                            maxScore = tempScore;
                        }
                    }
                }
                WeightsIndexs[i] = (double)maxIndex;
            }
        }
        return WeightsIndexs;
    }

    /// <summary>
    /// this function reads from the weights file the strongers thoughts
    /// </summary>
    /// <param name="NumberOfThoughts">the number of best thoughts index to return</param>
    /// <returns> a pointer(array) to array of pointer ([][]) that each array in the array of arrays contain an array of wegiths </returns>
    double[][] GetStrongest(int NumberOfThoughts)
    {
        double[][] RetWeights = new double[NumberOfThoughts][];
        for (int i = 0; i < NumberOfThoughts; i++)
        {
            RetWeights[i] = new double[48];
        }
        double[] WeightsIndexes = GetWeightsIndex(NumberOfThoughts);
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\Weights.bin", FileMode.Open)))
        {
            for (int i = 0; i < NumberOfThoughts; i++)
            {
                br.BaseStream.Seek(0, SeekOrigin.Begin);
                // 2.
                // Important variables:
                int length = (int)br.BaseStream.Length;
                int pos = 50000;
                int required = 2000;
                int count = 0;
                int currIndex = 0;
                // 3.
                // Seek the required index.
                //br.BaseStream.Seek(pos, SeekOrigin.Begin);
                currIndex = br.ReadInt32();
                while (currIndex != WeightsIndexes[i])
                {
                    br.BaseStream.Seek(48 * sizeof(double), SeekOrigin.Current);
                    currIndex = br.ReadInt32();
                }
                // 4.
                // Slow loop through the bytes.
                string wet = "";
                for (int k = 0; k < 48; k++)
                {
                    RetWeights[i][k] = br.ReadDouble();
                    wet += RetWeights[i][k].ToString() + ", ";
                }
                Debug.Log(i + " S Weights: " + wet);
            }
        }
        return RetWeights;
    }

    /// <summary>
    /// this function copy the best thought to the new gen
    /// </summary>
    /// <param name="fileName">the file to write into</param>
    /// <param name="numberOfBestThoughts">the number of best thought to copy</param>
    void CopyBestThoughtsToNewGen(string fileName, int numberOfBestThoughts)
    {
        BinaryWriter bw;
        FileStream fs;
        double[][] weights = GetStrongest(numberOfBestThoughts);
        try
        {
            fs = new FileStream("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Create);
            bw = new BinaryWriter(fs);
        }
        catch (IOException exe)
        {
            Console.WriteLine(exe.Message + "\\Cannot open file");
            return;
        }
        string wet = "";
        for (int j = 0; j < numberOfBestThoughts; j++)
        {
            bw.Write(j);
            for (int i = 0; i < 48; i++)
            {
                wet += weights[j][i].ToString() + ", ";
                bw.Write(weights[j][i]);
            }
            Console.WriteLine("Write to next gen: " + wet);
        }
        bw.Close();
        fs.Close();
    }

    /// <summary>
    /// this function creates and copt new random weights to the new gen
    /// </summary>
    /// <param name="fileName">the file to write into</param>
    /// <param name="startIndex">the start index of the new thoughts</param>
    /// <param name="NewThoughts">the number of new random thoughts wanted</param>
    void CopyCreateRandThoughtToNewGen(string fileName, int startIndex, int NewThoughts)
    {
        BinaryWriter bw;
        FileStream fs;
        try
        {
            fs = new FileStream("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Create);
            bw = new BinaryWriter(fs);
        }
        catch (IOException exe)
        {
            Console.WriteLine(exe.Message + "\\Cannot open file");
            return;
        }
        System.Random rnd = new System.Random();
        double[] weights = new double[48];
        string wet = "";
        for (int j = startIndex; j < startIndex+NewThoughts; j++)
        {
            bw.Write(j);
            for (int i = 0; i < 48; i++)
            {
                weights[i] = rnd.Next(-11, 11);
                wet += weights[i].ToString() + ", ";
                bw.Write(weights[i]);
            }
            Console.WriteLine("Write to next gen: " + wet);
        }
        bw.Close();
        fs.Close();
    }

    /// <summary>
    /// gets a thought out of a file by its index
    /// </summary>
    /// <param name="fileName"> the fiel to search in</param>
    /// <param name="ThoughtIndex"> the index of the </param>
    /// <returns></returns>
    double[] GetThoughtFromFile(string fileName, int ThoughtIndex)
    {
        double[] weights = new double[48];
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Open)))
        {
            // 2.
            // Important variables:
            int length = (int)br.BaseStream.Length;
            int pos = 50000;
            int required = 2000;
            int count = 0;
            int currIndex = 0;
            // 3.
            // Seek the required index.
            //br.BaseStream.Seek(pos, SeekOrigin.Begin);
            currIndex = br.ReadInt32();
            while (currIndex != ThoughtIndex)
            {
                br.BaseStream.Seek(48 * sizeof(double), SeekOrigin.Current);
                currIndex = br.ReadInt32();
            }
            // 4.
            // Slow loop through the bytes.
            string wet = "";
            for (int i = 0; i < 48; i++)
            {
                weights[i] = br.ReadDouble();
                wet += weights[i].ToString() + ", ";
            }
            Debug.Log("Thought Returnd for CreateOffspringFrom2Perents is: " + wet);
        }
        return weights;
    }

    /// <summary>
    /// this function craetes two offsprings from two perents and put then in aa metrix
    /// </summary>
    /// <param name="fileName">the file that contian all the weights</param>
    /// <param name="Perent1Index">index of first perent</param>
    /// <param name="Perent2Index">index of second perent</param>
    /// <returns> a matrix in a size of 2*48 - 2 offsprings that their size is 48 numbers (weights)</returns>
    double[][] CreateOffspringFrom2Perents(string fileName, int Perent1Index, int Perent2Index)
    {
        int iSswitch;
        System.Random rand = new System.Random();
        double[][] Perents = new double[2][];
        Perents[0] = GetThoughtFromFile(fileName, Perent1Index);
        Perents[1] = GetThoughtFromFile(fileName, Perent2Index);
        for (int i = 0; i < 48; i++)
        {
            iSswitch = rand.Next(2);
            if (iSswitch == 1)
            {
                Perents[0][i] += Perents[1][i];
                Perents[1][i] = Perents[0][i] - Perents[1][i];
                Perents[0][i] -= Perents[1][i];
            }
        }
        return Perents;
    }

    /// <summary>
    /// this funciton writes wegihts in to a file
    /// </summary>
    /// <param name="fileName">the file to write into</param>
    /// <param name="exsistThoughts">number of already existing thiughts in the file</param>
    /// <param name="ThoughtsToWrite"> nubmer of thoughts givien</param>
    /// <param name="Weights">a matrix that cintain the new thoughts</param>
    void WriteWeightsToFile(string fileName, int exsistThoughts, int ThoughtsToWrite, double[][] Weights)
    {
        BinaryWriter bw;
        FileStream fs;
        try
        {
            fs = new FileStream("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Create);
            bw = new BinaryWriter(fs);
        }
        catch (IOException exe)
        {
            Console.WriteLine(exe.Message + "\\Cannot open file");
            return;
        }
        System.Random rnd = new System.Random();
        string wet = "";
        for (int j = 0; j < ThoughtsToWrite; j++)
        {
            bw.Write(j+ exsistThoughts);
            for (int i = 0; i < 48; i++)
            {
                wet += Weights[j][i].ToString() + ", ";
                bw.Write(Weights[j][i]);
            }
            Console.WriteLine("index: " + j + " | Write to next gen: " + wet);
        }
        bw.Close();
        fs.Close();
    }

    /// <summary>
    /// this function uses CreateOffspringFrom2Perents, WriteWeightsToFile, ThoughtsRouletteWheelSelection
    /// to do the selection, muration, and next get part of GA
    /// </summary>
    /// <param name="FitFileName">the file name that contains all the fit scores of the thouvhts</param>
    /// <param name="weightFile">the file name that contain all the old gen weights</param>
    /// <param name="ToWriteFile">the name of the file that woll ocntain the new gen weights</param>
    /// <param name="exsistThoughts">already existing thoughts in the new gen</param>
    void CreateOffspringAndWriteToFile(string FitFileName, string weightFile,string ToWriteFile, int exsistThoughts)
    {
        int[] PerentsIndex = ThoughtsRouletteWheelSelection(FitFileName, (50 - exsistThoughts));
        for (int i = 0; i < PerentsIndex.Length; i+=2)
        {
            double[][] Offsprings = CreateOffspringFrom2Perents(weightFile, PerentsIndex[i], PerentsIndex[i+1]);
            WriteWeightsToFile(ToWriteFile, exsistThoughts + i, 2, Offsprings);
        }
    }

    /// <summary>
    /// this funciotn creates and writes into file the new gen thoughts
    /// </summary>
    /// <param name="ToWriteFile">the name of the file to write the new gen</param>
    /// <param name="ToReadFile"> the name of the file to read the old gen</param>
    /// <param name="numOfBestThoughts">number of nest thoughts to pass to new gen</param>
    /// <param name="numOfRandThought">number of random thoughts to pass to new gen</param>
    void WriteNewThoughtsToNextGen(string ToWriteFile, string ToReadFile, int numOfBestThoughts, int numOfRandThought)
    {
        WriteBestThoughtSession("BestOfEachGen.bin");
        CopyBestThoughtsToNewGen(ToWriteFile, numOfBestThoughts);//1
        CopyCreateRandThoughtToNewGen(ToWriteFile, numOfBestThoughts, numOfRandThought);//3
        CreateOffspringAndWriteToFile("FitnessScores.bin", ToReadFile, ToWriteFile, numOfBestThoughts + numOfRandThought);
    }

    /// <summary>
    /// this funtion writes the best thought from the old gen to a file that will ocntain all hte best of each gen
    /// </summary>
    /// <param name="fileName">ht ename of the file that will ocntain all hte best of each gen </param>
    void WriteBestThoughtSession(string fileName)
    {
        int CurrGen;
        using (BinaryReader br = new BinaryReader(File.Open("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\CurrGenNumber.bin", FileMode.Open)))
        {
            // 2.
            // Important variables:
            // 3.
            // Seek the required index.
            //br.BaseStream.Seek(pos, SeekOrigin.Begin);
            CurrGen = br.ReadInt32();
        }
        BinaryWriter bw;
        FileStream fs;
        try
        {
            fs = new FileStream("C:\\Users\\yotam\\Desktop\\AlphaCar\\Assets\\Scripts\\" + fileName, FileMode.Create);
            bw = new BinaryWriter(fs);
        }
        catch (IOException exe)
        {
            Console.WriteLine(exe.Message + "\\Cannot open file");
            return;
        }
        double[][] weights = GetStrongest(1);
        string wet = "";
        CurrGen++;
        bw.Write(CurrGen);
        for (int i = 0; i < 48; i++)
        {
            wet += weights[0][i].ToString() + ", ";
            bw.Write(weights[0][i]);
        }
        Console.WriteLine("Write to next gen: " + wet);
        bw.Close();
        fs.Close();
    }

    //called when ther is a Collision with the car hitbox
    //gets - collisioninfo - the Collision information
    void OnCollisionEnter(Collision collisioninfo)
    {
        //checks if the Collision accured with a Barrier
        if (collisioninfo.collider.tag == "Barrier")
        {
            //print and log the collision basic info
            //Debug.Log("Crash in:" + collisioninfo.collider.name);
            //disable the car
            //movement.enabled = false;
            //restart spot
            //movement.transform.position = new Vector3((float)-178.59, (float)2.6, (float)162.12);
            //movement.transform.eulerAngles = new Vector3((float)0, (float)0, (float)0);
            //restart game
            if (!collisioninfo.collider.name.Equals("Barrier (11)") && !collisioninfo.collider.name.Equals("Barrier (27)"))
            {
                check.SetWeightSetIndex(check.GetWeightSetIndex() + 1);
                Debug.Log("index: " + check.GetWeightSetIndex().ToString());
                if (check.GetWeightSetIndex() <= 49)
                    SceneManager.LoadScene("SandBox");
                else
                {
                    WriteNewThoughtsToNextGen("TempData.bin", "Weights.bin", 1, 3);
                    check.enabled = false;
                }
            }
            else if(collisioninfo.collider.name.Equals("Barrier (158)"))
            {
                check.SetNextMove("left");
            }
            /////////////////////////////////
            //else if (collisioninfo.collider.name.Equals("Barrier (11)"))
            //{
            //    check.SetNextMove("right_10");
            //}
            //else
            //{
            //    check.SetNextMove("right_7");
            //}
            //////////////////////////////////
            //else if(collisioninfo.collider.name.Equals("Barrier (11)"))
            //{
            //    check.SetNextMove("right_10");
            //}
            else
            {
                check.SetNextMove("right_10");
            }
        }
    }
}
