# Flat-CSV-to-JSON
Converts a variable width, flat CSV file to a hierarchical JSON file. 
  * CSV file has one File record, designated "F" and one End record, designated "E"
  * Each F record can contain multiple Order records, designated "O".
  * Each O record contains a single "B" & "T" records
  * Each O record can contain multiple Line Item records, designated "L"
