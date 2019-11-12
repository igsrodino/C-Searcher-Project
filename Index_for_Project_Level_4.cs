using System;
using System.IO;
using System.Collections.Generic;
using System.Text;


namespace ConsoleApp2
{
    class Index
    {
        public static void IndexSequencer(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read); // Read fasta file
            StreamReader reader = new StreamReader(fs); 
            FileStream outfile = new FileStream("16S.index", FileMode.Create, FileAccess.Write); // Create and write index file


            int counter = 0; // Number of lines seen so far
            string line; // Each line equals a full line of text
            int position = 0; // File position of first line
            List<int> pos = new List<int>(); // array to keep line positions in
            List<int> size = new List<int>(); //array for size of line
            List<string> sequences = new List<string>(); // array to keep sequences in
            string[] sequences_list; // Array for storing sequences

            while ((line = reader.ReadLine()) != null) // While its not the end of the file
            {
                pos.Insert(counter, position); // To store line position
                position = position + line.Length + 1; // Increment file position by line length
                sequences_list = line.Split('>'); // Split lines that have >
                int offset = pos[counter]; // Store position count
                counter++; // Increment number of lines seen so far

                for (int i = 1; i < sequences_list.Length; i++) // Loop for length of list
                {
                    string seq = sequences_list[i]; // To store whole list
                    string seq_IDs = seq.Split(' ')[0]; // To grab only sequence ID's
                    sequences.Add(seq_IDs + ' ' + offset + '\n'); // Add sequence ID's and offset number to list, ready for writing to file
                }

            }

            using (StreamWriter writer = new StreamWriter(outfile)) // Write to file
            {
                sequences.ForEach(writer.Write); // Write sequences into writer file
            }
        }

		// Main part of program to run index part
        static void Main(String[] args)
        {
            try
            {
                string filename = args[0]; // First and only argument for the fasta file

                if (args.Length == 1) // For error handling
                {
                    IndexSequencer(filename);
                }
                else
                {

                    // Error handling for if not enough arguments are entered. 
                    Console.WriteLine("\nSorry, too many arguments entered, you must enter 1 arguments for Level 1." +
                        "\n\nPlease use this example as a guide: 'IndexSequencer 16S.fasta'");
                }
            }

            // Error handling for ensuring correct format is entered for the last 2 arguments.
            catch (FileNotFoundException)
            {
                Console.WriteLine("\nError, file could not be found." +
                    " Please check file name and directory, and try again.");
            }
			// Error handling for number of arguments entered
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("\nError, Not enough arguments entered." +
                    "\n\nPlease use this example as a guide: 'IndexSequencer 16S.fasta'");
            }


        }
    }
}

