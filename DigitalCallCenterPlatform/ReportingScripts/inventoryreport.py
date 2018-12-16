#!/usr/bin/python3

from reportsmodule import *

SQLCommand = """ SELECT [Id], [ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], FORMAT([PalacementDate],'yyyy-MM-dd'), FORMAT([LastWorkDate],'yyyy-MM-dd HH:MM:ss') FROM [dbo].[WorkPlatformModels] WHERE ClientID in %s ORDER BY PalacementDate DESC"""

if len(sys.argv) > 1:
    client_id = sys.argv[1]
else:
    client_id = ""

print (str(sys.argv))

cursor.execute(SQLCommand % client_id) 

results = cursor.fetchone() 

workbook = xlsxwriter.Workbook('C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\ReportingFolder\\Inventory_Rerpot.xlsx')
worksheet = workbook.add_worksheet()

# Set column width.
worksheet.set_column(0, 0, 15)
worksheet.set_column(1, 1, 15)
worksheet.set_column(2, 2, 10)
worksheet.set_column(3, 4, 20)
worksheet.set_column(4, 4, 16)
worksheet.set_column(5, 5, 16)
worksheet.set_column(6, 6, 10)
worksheet.set_column(7, 7, 10)
worksheet.set_column(8, 7, 8)
worksheet.set_column(9, 8, 8)
worksheet.set_column(10, 10, 16)
worksheet.set_column(11, 11, 16)

# Format
f_text_center_tahoma_8 = workbook.add_format(text_center_tahoma_8)
f_headers_format = workbook.add_format(headers_format)
f_text_left_tahoma_8 = workbook.add_format(text_left_tahoma_8)

# Header list.
headers = ['Account Number', 'Client Reference', 'Client ID', 'Name', 'Assign Amount', 'Total Received', 'Other Due', 'Total Due', 'Desk', 'Status', 'Placement Date', 'Last Work Date']

# Writing header.
writeHeader(worksheet, 0, 0, headers, format=f_headers_format)

row = 1

while results:
    worksheet.write(row, 0, str(results[0]), f_text_left_tahoma_8)
    worksheet.write(row, 1, str(results[1]), f_text_left_tahoma_8)
    worksheet.write(row, 2, str(results[2]), f_text_center_tahoma_8)
    worksheet.write(row, 3, str(results[3]), f_text_center_tahoma_8)
    worksheet.write(row, 4, str(results[4]), f_text_left_tahoma_8)
    worksheet.write(row, 5, str(results[5]), f_text_left_tahoma_8)
    worksheet.write(row, 6, str(results[6]), f_text_left_tahoma_8)
    worksheet.write(row, 7, str(results[7]), f_text_left_tahoma_8)
    worksheet.write(row, 8, str(results[8]), f_text_center_tahoma_8)
    worksheet.write(row, 9, str(results[9]), f_text_center_tahoma_8)
    worksheet.write(row, 10, str(results[10]), f_text_center_tahoma_8)
    worksheet.write(row, 11, str(results[11]), f_text_center_tahoma_8)

    row += 1

    results = cursor.fetchone() 

connection.close()

workbook.close()