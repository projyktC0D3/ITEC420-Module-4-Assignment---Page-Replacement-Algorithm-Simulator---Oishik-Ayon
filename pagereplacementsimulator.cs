using System;
using System.Collections.Generic;
using System.Linq;
class Program
{
    static void Main()
    {
        Console.Write("Enter Reference String (e.g. 1 2 3 1 2): ");
        string input = Console.ReadLine();
        input = input.Replace(",", " ");
        int[] referenceString = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();
        Console.Write("Enter Frame Count: ");
        int frameCount = int.Parse(Console.ReadLine());
        if (frameCount <= 0)
        {
            Console.WriteLine("The number of frames must be greater than 0!!!");
            return;
        }
        Console.WriteLine();
        RunFIFO(referenceString, frameCount);
        RunLRU(referenceString, frameCount);
        RunOptimal(referenceString, frameCount);
    }
    static void RunFIFO(int[] referenceString, int frameCount)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("| FIFO PAGE REPLACEMENT                             |");
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Page\tFrames\t\tResult");
        List<int> frames = new List<int>();
        int pageFaults = 0;
        int nextToReplace = 0;
        foreach (int page in referenceString)
        {
            bool hit = frames.Contains(page);
            if (!hit)
            {
                pageFaults++;
                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                }
                else
                {
                    frames[nextToReplace] = page;
                    nextToReplace = (nextToReplace + 1) % frameCount;
                }
            }
            PrintStep(page, frames, frameCount, hit);
        }
        Console.WriteLine();
        Console.WriteLine("Total Page Faults: " + pageFaults);
        Console.WriteLine();
    }
    static void RunLRU(int[] referenceString, int frameCount)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("| LRU PAGE REPLACEMENT                              |");
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Page\tFrames\t\tResult");
        List<int> frames = new List<int>();
        int pageFaults = 0;
        foreach (int page in referenceString)
        {
            bool hit = frames.Contains(page);
            if (hit)
            {
                frames.Remove(page);
                frames.Add(page);
            }
            else
            {
                pageFaults++;
                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                }
                else
                {
                    frames.RemoveAt(0);
                    frames.Add(page);
                }
            }
            PrintStep(page, frames, frameCount, hit);
        }
        Console.WriteLine();
        Console.WriteLine("Total Page Faults: " + pageFaults);
        Console.WriteLine();
    }
    static void RunOptimal(int[] referenceString, int frameCount)
    {
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("| OPTIMAL PAGE REPLACEMENT                          |");
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("Page\tFrames\t\tResult");
        List<int> frames = new List<int>();
        int pageFaults = 0;
        for (int i = 0; i < referenceString.Length; i++)
        {
            int page = referenceString[i];
            bool hit = frames.Contains(page);
            if (!hit)
            {
                pageFaults++;
                if (frames.Count < frameCount)
                {
                    frames.Add(page);
                }
                else
                {
                    int indexToReplace = FindOptimalReplacement(
                        referenceString,
                        frames,
                        i + 1
                    );
                    frames[indexToReplace] = page;
                }
            }
            PrintStep(page, frames, frameCount, hit);
        }
        Console.WriteLine();
        Console.WriteLine("Total Page Faults: " + pageFaults);
        Console.WriteLine();
    }
    static int FindOptimalReplacement(
        int[] referenceString,
        List<int> frames,
        int startIndex)
    {
        int indexToReplace = 0;
        int farthestUse = -1;
        for (int i = 0; i < frames.Count; i++)
        {
            int page = frames[i];
            int nextUse = -1;
            for (int j = startIndex; j < referenceString.Length; j++)
            {
                if (referenceString[j] == page)
                {
                    nextUse = j;
                    break;
                }
            }
            if (nextUse == -1)
            {
                return i;
            }
            if (nextUse > farthestUse)
            {
                farthestUse = nextUse;
                indexToReplace = i;
            }
        }
        return indexToReplace;
    }
    static void PrintStep(
        int page,
        List<int> frames,
        int frameCount,
        bool hit)
    {
        string frameContents = "";
        for (int i = 0; i < frameCount; i++)
        {
            if (i < frames.Count)
            {
                frameContents += frames[i];
            }
            else
            {
                frameContents += "-";
            }
            if (i < frameCount - 1)
            {
                frameContents += " ";
            }
        }
        string result = hit ? "HIT" : "FAULT";
        Console.WriteLine($"{page}\t{frameContents}\t\t{result}");
    }
}
