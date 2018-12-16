#!/usr/bin/python3

from reportsmodule import *

SQLCommand = """SELECT FORMAT(NoteDate,'yyyy-MM-dd HH:MM:ss'), UserCode, Desk, ActionCode, Status, Note FROM NotesModels WHERE AccountNumber = '%s' ORDER BY SeqNumber"""

if len(sys.argv) > 1:
    account_id = sys.argv[1]
else:
    account_id = ""

print (str(sys.argv))

cursor.execute(SQLCommand % account_id) 

results = cursor.fetchone() 

workbook = xlsxwriter.Workbook('C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\ReportingFolder\\Notes_Rerpot.xlsx')
worksheet = workbook.add_worksheet()

# Set column width.
worksheet.set_column(0, 0, 20)
worksheet.set_column(1, 1, 10)
worksheet.set_column(2, 2, 10)
worksheet.set_column(3, 4, 12)
worksheet.set_column(4, 4, 12)
worksheet.set_column(5, 5, 100)

# Format
f_text_center_tahoma_8 = workbook.add_format(text_center_tahoma_8)
f_headers_format = workbook.add_format(headers_format)
f_text_left_tahoma_8 = workbook.add_format(text_left_tahoma_8)

# Header list.
headers = ['Note Date', 'User Code', 'Desk', 'Action Code', 'Status', 'Note']

# Writing header.
writeHeader(worksheet, 0, 0, headers, format=f_headers_format)

row = 1

while results:
    worksheet.write(row, 0, str(results[0]), f_text_center_tahoma_8)
    worksheet.write(row, 1, str(results[1]), f_text_center_tahoma_8)
    worksheet.write(row, 2, str(results[2]), f_text_center_tahoma_8)
    worksheet.write(row, 3, str(results[3]), f_text_center_tahoma_8)
    worksheet.write(row, 4, str(results[4]), f_text_center_tahoma_8)
    worksheet.write(row, 5, str(results[5]), f_text_left_tahoma_8)

    row += 1

    results = cursor.fetchone() 

connection.close()

workbook.close()