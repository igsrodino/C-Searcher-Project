using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;


namespace Searcher_Project
{
    // Class name for this program.
    class Project
    {
        // Level 1 method, links with in main method
        public static void Level1(string filename, int starting_point, int amount_to_show)
        {
            int counter = 0; // Number of lines seen so far
            string line; // Each line equals a full line of text
            int position = 0; // File position of first line
            List<int> pos = new List<int>(); // array to keep line positions in
            List<int> size = new List<int>(); //array for size of line

            // To read given file and display line by line
            StreamReader file = new StreamReader(filename);

            // While its not the end of the file
            while ((line = file.ReadLine()) != null)
            {
                pos.Insert(counter, position); // To store line position
                size.Insert(counter, line.Length + 1); // To store line size
                counter++; // Increment number of lines seen so far
                position = position + line.Length + 1; // Increment file position by line length
            }

            // Close file
            file.Close();

            // To count maximum number of lines ini the file.
            int max_lines = counter - 1;

            // Open and read given file then do given instructions.
            using (FileStream DNAFile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                // To count number of sequences as a sequence is more than the one line.
                int number_of_sequences = ((starting_point + (amount_to_show * 2)) - 1);

                try
                {
                    // Error handling to make sure user requests/inputs 1 or more sequences
                    if (amount_to_show > 0)
                    {
                        // For loop to start from given line, and to search only up to wanted sequence. Increment by one each loop.
                        for (int n = (starting_point - 1); n < number_of_sequences; n++)
                        {
                            // Error handling to ensure search is within file line size
                            if (starting_point > (counter))
                            {
                                Console.WriteLine($"\nError, end of file. File is only {max_lines}" +
                                    $" lines long. Search must start and end before the end of file.");
                                break;
                            }

                            // Error handling to ensure user inputs a number greater than 0 for search starting point.
                            else if (starting_point < 1)
                            {
                                Console.WriteLine("\nError, search must start from a number greater than 0. Please try again.");
                                break;
                            }

                            // Error handling to ensure user inputs an odd number for starting point.
                            else if (starting_point % 2 == 0)
                            {
                                Console.WriteLine("\nError, search must start from an odd number. Please try again.");
                                break;
                            }

                            else
                            {
                                // To count line position and size from the start of the file
                                byte[] bytes = new byte[size[n]];
                                DNAFile.Seek(pos[n], SeekOrigin.Begin);
                                DNAFile.Read(bytes, 0, size[n]);
                                Console.Write(Encoding.Default.GetString(bytes));
                            }
                        }
                    }

                    // To ensure user searches a sequence amount that isn't 0
                    else if (amount_to_show == 0)
                    {
                        Console.WriteLine("\nError, number of sequences searched cannot be a 0.");
                    }

                    // To ensure user searches sequence amount that is not a negative number
                    else if (amount_to_show < 0)
                    {
                        Console.WriteLine("\nError, number of sequences searched must be a positive number.");
                    }
                }

                // Error handling will tell the user that it is the end of the file and they have searched past it.
                catch (ArgumentOutOfRangeException)
                {
                    int lines_over = max_lines - (number_of_sequences - 1);
                    Console.WriteLine($"\n\nError, end of file. '{filename}' is {max_lines}" +
                        $" lines long, and you have searched {-lines_over} lines" +
                        $" past this maximum line count for this file. No more DNA Sequences " +
                        $"to show. ");
                }
            }
        }



        // Level 2 method, links with main method
        public static void Level2(string filename, string sequence_Id)
        {
            string line; // Each line equals a full line of text

            // File stream from the file name
            FileStream DNAFile = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // To read given file and display line by line
            using (StreamReader file = new StreamReader(DNAFile))
            {
                bool parsed = true; // Boolean is required for finding sequences that were either found or not found in search.

                if (parsed == true)
                {
                    // While it is not the end of the file
                    while ((line = file.ReadLine()) != null)
                    {

                        // if it contains id -> print
                        if (line.Contains(sequence_Id))
                        {
                            parsed = true;
                            Console.WriteLine(line);
                            line = file.ReadLine();
                            Console.WriteLine(line);
                            break;
                        }
                        // Needed for error handling
                        else
                        {
                            parsed = false;
                        }
                    }
                }

                // Error handling for when a sequence searched can't be found
                if (parsed == false)
                {
                    Console.WriteLine($"\nSorry, DNA Sequence '{sequence_Id}' " +
                       "cannot be found, please check the spelling and try again");
                }
            }
        }



        // Level 3 method, links with main method
        public static void Level3(string filename, string query_file, string results_file)
        {

            // To write created file line by line, will overwrite file if results_file name is the same as on the last search.
            using (StreamWriter output_file = new StreamWriter(results_file))
            {

                // To read each line in a loop in query file given
                foreach (string queries_to_search in File.ReadLines(query_file))
                {
                    // Required booleans used to help store search query results is in the DNA file(filename) or not
                    bool found = false;
                    bool success = false;

                    // To read each line in a loop in main file given (filename)
                    foreach (string sequences_found in File.ReadLines(filename))
                    {
                        // To store/write ID searched with data in results file(output_file).
                        if (success == true)
                        {
                            output_file.WriteLine(sequences_found);
                            success = false;
                        }

                        // Required for error handling, to store sequences not found in search
                        if (sequences_found.Contains(queries_to_search))
                        {
                            found = true;
                            success = true;
                            output_file.WriteLine(sequences_found);
                        }
                    }

                    // Error message to let user know the searched ID not found (if any).
                    if (found == false)
                    {
                        Console.WriteLine($"\nError, sequence: '{queries_to_search}' not found. Please check spelling and try again." +
                            $"\n\nAll sequences found have been stored in '{results_file}' file.");
                    }
                }
            }
        }


        // Level 4 method, links with main method
        public static void Level4(string filename, string index_file, string query_file, string results_file)
        {
            string sequences_to_print = "";
            List<string> sequences_output = new List<string>();
            FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StreamReader fs = new StreamReader(file);
            
                foreach (string sequences_found in File.ReadAllLines(index_file))
                {
                    string[] indexed_offset; // array for offset numbers and id
                    indexed_offset = sequences_found.Split(' '); // Split results into an array

                    string ID_for_offset = indexed_offset[0];  // Store [0] of split array and store in offset
                    string offset = indexed_offset[1];  // Store [1] of split array and store in offset
                    long offset_seek = Convert.ToInt64(offset);  // Convert offset from string to long for byte search

                    string ID_line, sequence_line;  // Strings to store ID and sequence part of DNA sequence*/

                    foreach (string queries_to_search in File.ReadLines(query_file))  // Loop to search through query file
                    {
                        // Required booleans used to help store search query results is in the DNA file(filename) or not
                        //bool found = false;
                        bool success = false;

                        if (sequences_found.Contains(queries_to_search)) // Compare queries with sequences in index file
                        {
                            file.Seek(offset_seek, SeekOrigin.Begin); //Start searching with offset number from index file
                            ID_line = fs.ReadLine(); // Read id line and store
                            sequence_line = fs.ReadLine();  // Read following line and store
                            fs.DiscardBufferedData();  // Get rid of buffered data, if this isn't done, search shows up incorrectly.

                        // To store/write ID searched with data in results file(output_file).
                        if (success == true)
                            {
                            sequences_to_print = (ID_line + '\n' + sequence_line + '\n'); // Add id and sequence into list for printing to file.
                            sequences_output.Add(sequences_to_print); // Add found sequences to output list
                            success = false;
                            }
                        // Required for error handling, to store sequences not found in search
                        if (ID_line.Contains(queries_to_search))
                            {
                           // found = true;
                            success = true;
                            sequences_to_print = (ID_line + '\n' + sequence_line); // Add id and sequence into list for printing to file.
                            sequences_output.Add(sequences_to_print); // Add found sequences to output list
                                
                            }
                        }
                    }

                    using (StreamWriter writer = File.CreateText(results_file)) // Create and write on file
                    {
                        foreach (string all_lines in sequences_output) // Loop for printing output to file
                        {
                            writer.WriteLine(all_lines); // Print each line to results file
                        }
                    }  
            }
            
        }

        // Level 5 method, links with main method
        public static void Level5(string filename, string search)
        {
           // string line; // Each line equals a full line of text
            string DNA_ID_line = ""; // To store DNA ID line, as search will be from DNA sequence line (the line after ID)
            bool found = false; // Used for error handling

                foreach (string DNA_sequence_line in File.ReadLines(filename)) // Loop to search through fasta file
                {
                    if (DNA_sequence_line.Contains(search)) // Check search items in fasta file
                    {
                        Console.WriteLine(DNA_ID_line.Substring(1, 11));
                        found = true;
                    }

                    DNA_ID_line = DNA_sequence_line;
                }

            // Error message to let user know the searched string is not found (if any).
            if (found == false)
            {
                Console.WriteLine($"\nError: '{search}' not found. Please check spelling and try again.");
            }     
        }



        // Level 6 method, links with main method
        public static void Level6(string filename, string search)
        {
            string line; // Each line equals a full line of text
  
            // File stream from the file name
            FileStream DNAFile = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // To read given file and display line by line
            using (StreamReader file = new StreamReader(DNAFile))
            {
                bool found = false; // Boolean is required for error handling to check if search is found or not.
                
                // While it is not the end of the file
                while ((line = file.ReadLine()) != null)
                {
                    // if it contains id -> print
                    if (line.Contains(search))
                    {
                        found = true;
                        // To show ID number, minus the > at the start, and only for 11 characters 
                        // long as all ID's are that long.
                        string ID_only = line.Substring(1, 11); 
                        Console.WriteLine(ID_only);
                    }
                }
                 
                // Error message to let user know the searched string is not found (if any).
                if (found == false)
                {
                    Console.WriteLine($"\nError: '{search}' not found. Please check spelling and try again.");
                }               
            }
        }

        

        // Main method, used to interact with level methods
        static void Main(String[] args)
        {
            // These first 2 arguments are to receive command for level and file name wanted to be used.
            string level = args[0];
            string filename = args[1];

            // For error handling too see if right input is entered
            if (level == "-level1" || level == "-level2" || level == "-level3" || level == "-level4" || level == "-level5" || level == "-level6")
            {
                try
                {
                    // Switch is to be able to switch between levels (in their own classes) by using the first argument
                    switch (level)
                    {

                        case "-level1":

                            try
                            {
                                Console.WriteLine("Level 1");
                                // Following is for error handling check
                                if (args.Length == 4)
                                {
                                    // Args are to receive user inputs/commands and link with Level 1.
                                    int starting_point = int.Parse(args[2]);
                                    int amount_to_show = int.Parse(args[3]);
                                    Level1(filename, starting_point, amount_to_show);
                                }
                                else
                                {
                                    // Error handling for if not enough arguments are entered. 
                                    Console.WriteLine("\nSorry, not enough arguments entered, you must enter 4 arguments for Level 1." +
                                        "\n\nPlease use this example as a guide: '-level1 16S.fasta 273 1'");
                                }
                            }

                            // Error handling for ensuring correct format is entered for the last 2 arguments.
                            catch (FormatException)
                            {
                                Console.WriteLine("\nError, number of lines and number of sequences" +
                                    " can only be searched as positive whole integers. Please try again.");
                            }
                            break;


                        case "-level2":

                            Console.WriteLine("Level 2");
                            // Following is for error handling check
                            if (args.Length == 3)
                            {
                                // Arg is to read user input and link with Level 2.
                                string sequence_Id = args[2];
                                Level2(filename, sequence_Id);
                            }
                            else
                            {
                                // Error handling for if not enough arguments are entered. 
                                Console.WriteLine("\nSorry, not enough arguments entered, you must enter 3 arguments for Level 2." +
                                    "\n\nPlease use this example as a guide: ' -level2 16S.fasta NR_115365.1'");
                            }
                            break;


                        case "-level3":

                            try
                            {
                                Console.WriteLine("Level 3");
                                // Following is for error handling check
                                if (args.Length == 4)
                                {
                                    // Args are to receive user inputs/commands and link with Level 3.
                                    string query_file = args[2];
                                    string results_file = args[3];
                                    Level3(filename, query_file, results_file);
                                }
                                else
                                // Error handling for if not enough arguments are entered.
                                {
                                    Console.WriteLine("\nSorry, not enough arguments entered, you must enter 4 arguments for Level 3." +
                                        "\n\nPlease use this example as a guide: '-level3 16S.fasta query.txt results.txt'");
                                }
                            }
                            // Error handling for if file isn't found or is in incorrect format.
                            catch (FileNotFoundException)
                            {
                                Console.WriteLine("\nFile not found. Please check that both '16S.fasta' and 'query.txt' files are in the same " +
                                    "directory as this program, or debug folder if using a debugger, and then try again." +
                                    "\n\nAlternatively, please check that files are in correct format, first file must end with '.fasta' and" +
                                    " second file must end with '.txt'." +
                                    "\n\nThird file must end with either '.fasta' or '.txt'. If it does not, file will still" +
                                    " be created but format must be changed to either of those formats in order to view contents.");
                            }
                            break;


                        case "-level4":

                            try
                            {
                                // Following is for error handling check
                                if (args.Length == 5)
                                {
                                    // Args are to receive user inputs/commands and link with Level 3.
                                    string index_file = args[2];
                                    string query_file = args[3];
                                    string results_file = args[4];
                                    Console.WriteLine($"Level 4 \n \nAll sequences found have been stored in '{results_file}' file.");
                                    Level4(filename, index_file, query_file, results_file);
                                }
                                else
                                // Error handling for if not enough arguments are entered.
                                {
                                    Console.WriteLine("\nSorry, not enough arguments entered, you must enter 5 arguments for Level 4." +
                                        "\n\nPlease use this example as a guide: '-level3 16S.fasta 16S.index query.txt results.txt'");
                                }
                            }
                            // Error handling for if file isn't found or is in incorrect format.
                            catch (FileNotFoundException)
                            {
                                Console.WriteLine("\nFile not found. Please check that both '16S.fasta' and 'query.txt' files are in the same " +
                                    "directory as this program, or debug folder if using a debugger, and then try again." +
                                    "\n\nAlternatively, please check that files are in correct format, first file must end with '.index'," +
                                    " second file must end with '.fasta' and third file must end with '.txt'." +
                                    "\n\nThird file must end with either '.fasta' or '.txt'. If it does not, file will still" +
                                    " be created but format must be changed to either of those formats in order to view contents.");
                            }
                            break;


                        case "-level5":

                            Console.WriteLine("Level 5");
                            // Following is for error handling check
                            if (args.Length == 3)
                            {
                                // Args are to receive user inputs/commands and link with Level 3.
                                string search = args[2];
                                Level5(filename, search);
                            }
                            else
                            // Error handling for if incorrect number of arguments are entered.
                            {
                                Console.WriteLine("\nSorry, incorrect number of arguments entered, you must enter 3 arguments for Level 6." +
                                    "\n\nPlease use this example as a guide: '-level6 16S.fasta CTGGTACGGTCAACTTGCTCTAAG'");
                            }
                            break;


                        case "-level6":

                            Console.WriteLine("Level 6");
                            // Following is for error handling check
                            if (args.Length == 3)
                            {
                                // Args are to receive user inputs/commands and link with Level 3.
                                string search = args[2];
                                Level6(filename, search);
                            }
                            else
                            // Error handling for if not enough arguments are entered.
                            {
                                Console.WriteLine("\nSorry, not enough arguments entered, you must enter 3 arguments for Level 6." +
                                    "\n\nPlease use this example as a guide: '-level6 16S.fasta Streptomyces'");
                            }
                            break;


                        case "-level7":

                            Console.WriteLine("Level 7");
                            // Following is for error handling check
                            if (args.Length == 3)
                            {
                                // Args are to receive user inputs/commands and link with Level 3.
                                string search = args[2];
                                Level7(filename, search);
                            }
                            else
                            // Error handling for if not enough arguments are entered.
                            {
                                Console.WriteLine("\nSorry, not enough arguments entered, you must enter 3 arguments for Level 6." +
                                    "\n\nPlease use this example as a guide: '-level7 16S.fasta ACTG*GTAC*CA'");
                            }
                            break;
                    }
                }

                // System exception error handling when it can't find the file:
                catch (FileNotFoundException)
                {
                    Console.WriteLine("\nFile not found. Please check that '16S.fasta' file is in the same " +
                        "directory as this program, or debug folder if using a debugger, and then try again." +
                        "\n\nAlternatively, please check file is in correct format, file name must end with '.fasta'");
                }

                // System exception error handling for when an error isn't caught by other error handling
                // This error will output the actual error code so user can still see what happened:
                catch (IOException finalError)
                {
                    Console.WriteLine("\nCan't open file. Please make sure '16S.fasta' file is in the correct " +
                        "format(.fasta) and try again.\nIf format is correct, please see system's output for this " +
                        $"error: \n\n{finalError}");
                }

            }
            else
            // Error handling for if level entered is in the wrong format, or if level doesn't exist.
            {
                Console.WriteLine("\nSorry, could not find wanted level. Please ensure level wanted" +
                    " is written in the same format as the following example: '-level1' (If level 1" +
                    " is wanted, otherwise change the last digit to required level ie. 2, 3 or 4, etc...).");
            }
            Console.ReadLine();
        }
    }
}

