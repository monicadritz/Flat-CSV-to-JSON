# Flat-CSV-to-JSON
Converts a variable width, flat CSV file to a hierarchical JSON file. 
  * CSV file has one File record, designated "F" and one End record, designated "E"
  * Each F record can contain multiple Order records, designated "O".
  * Each O record contains a single "B" & "T" records
  * Each O record can contain multiple Line Item records, designated "L"

## Accompanying Notes 
 There were a couple of assumptions made about the structure of the data given in other potential CSV files. 
  * The input file will be named test.csv and in the current directory
  * Each record type would have the same number of fields 
  * The fields in each record would be in the same order
  * All fields must be quoted
  
