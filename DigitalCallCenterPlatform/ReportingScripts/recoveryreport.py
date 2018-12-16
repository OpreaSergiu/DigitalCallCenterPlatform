#!/usr/bin/python3

from reportsmodule import *

SQLCommand_active = """ SELECT [Id], [ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], FORMAT([PalacementDate],'yyyy-MM-dd'), FORMAT([LastWorkDate],'yyyy-MM-dd HH:MM:ss') FROM [dbo].[WorkPlatformModels] WHERE ClientID in %s AND Status = 'OPEN' ORDER BY PalacementDate DESC"""

SQLCommand_closed = """ SELECT [Id], [ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], FORMAT([PalacementDate],'yyyy-MM-dd'), FORMAT([LastWorkDate],'yyyy-MM-dd HH:MM:ss') FROM [dbo].[WorkPlatformModels] WHERE ClientID  in %s AND Status = 'CLOSED' ORDER BY PalacementDate DESC"""

SQLCommand_pending = """ SELECT [Id], [ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], FORMAT([PalacementDate],'yyyy-MM-dd'), FORMAT([LastWorkDate],'yyyy-MM-dd HH:MM:ss') FROM [dbo].[WorkPlatformModels] WHERE ClientID in %s AND Status = 'PENDING' ORDER BY PalacementDate DESC"""

SQLCommand_hold = """ SELECT [Id], [ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], FORMAT([PalacementDate],'yyyy-MM-dd'), FORMAT([LastWorkDate],'yyyy-MM-dd HH:MM:ss') FROM [dbo].[WorkPlatformModels] WHERE ClientID in %s AND Status = 'HOLD' ORDER BY PalacementDate DESC"""

if len(sys.argv) > 1:
	client_id = sys.argv[1]
else:
	client_id = ""

print (str(sys.argv))

workbook = xlsxwriter.Workbook('C:\\Users\\Sergiu\\source\\repos\\DigitalCallCenterPlatform\\DigitalCallCenterPlatform\\ReportingFolder\\Recovery_Rerpot.xlsx')

# Format
f_text_center_tahoma_8 = workbook.add_format(text_center_tahoma_8)
f_headers_format = workbook.add_format(headers_format)
f_text_left_tahoma_8 = workbook.add_format(text_left_tahoma_8)
f_percent = workbook.add_format(percent)

summary = workbook.add_worksheet('Summary')
active = workbook.add_worksheet('ACTIVE')
closed = workbook.add_worksheet('CLOSED')
pending = workbook.add_worksheet('PENDING')
hold = workbook.add_worksheet('HOLD') 

active_number_of_accounts = 0
closed_number_of_accounts = 0
pending_number_of_accounts = 0
hold_number_of_accounts = 0

active_total_assign_amt = 0
closed_total_assign_amt = 0
pending_total_assign_amt = 0
hold_total_assign_amt = 0

active_total_collected_amt = 0
closed_total_collected_amt = 0
pending_total_collected_amt = 0
hold_total_collected_amt = 0

active_current_balance = 0
closed_current_balance = 0
pending_current_balance = 0
hold_current_balance = 0

# Set column width.
active.set_column(0, 0, 15)
active.set_column(1, 1, 15)
active.set_column(2, 2, 10)
active.set_column(3, 4, 20)
active.set_column(4, 4, 16)
active.set_column(5, 5, 16)
active.set_column(6, 6, 10)
active.set_column(7, 7, 10)
active.set_column(8, 7, 8)
active.set_column(9, 8, 8)
active.set_column(10, 10, 16)
active.set_column(11, 11, 16)

# Header list.
headers = ['Account Number', 'Client Reference', 'Client ID', 'Name', 'Assign Amount', 'Total Received', 'Other Due', 'Total Due', 'Desk', 'Status', 'Placement Date', 'Last Work Date']

# Writing header.
writeHeader(active, 0, 0, headers, format=f_headers_format)

row = 1

cursor.execute(SQLCommand_active % client_id) 

results = cursor.fetchone() 

while results:
	active.write(row, 0, str(results[0]), f_text_left_tahoma_8)
	active.write(row, 1, str(results[1]), f_text_left_tahoma_8)
	active.write(row, 2, str(results[2]), f_text_center_tahoma_8)
	active.write(row, 3, str(results[3]), f_text_center_tahoma_8)
	active.write(row, 4, str(results[4]), f_text_left_tahoma_8)
	active.write(row, 5, str(results[5]), f_text_left_tahoma_8)
	active.write(row, 6, str(results[6]), f_text_left_tahoma_8)
	active.write(row, 7, str(results[7]), f_text_left_tahoma_8)
	active.write(row, 8, str(results[8]), f_text_center_tahoma_8)
	active.write(row, 9, str(results[9]), f_text_center_tahoma_8)
	active.write(row, 10, str(results[10]), f_text_center_tahoma_8)
	active.write(row, 11, str(results[11]), f_text_center_tahoma_8)

	active_number_of_accounts += 1

	active_total_assign_amt += float(results[4])

	active_total_collected_amt += float(results[5])

	active_current_balance += float(results[7])

	row += 1

	results = cursor.fetchone() 

#----------------------------------------------------------------------------

# Set column width.
closed.set_column(0, 0, 15)
closed.set_column(1, 1, 15)
closed.set_column(2, 2, 10)
closed.set_column(3, 4, 20)
closed.set_column(4, 4, 16)
closed.set_column(5, 5, 16)
closed.set_column(6, 6, 10)
closed.set_column(7, 7, 10)
closed.set_column(8, 7, 8)
closed.set_column(9, 8, 8)
closed.set_column(10, 10, 16)
closed.set_column(11, 11, 16)

# Header list.
headers = ['Account Number', 'Client Reference', 'Client ID', 'Name', 'Assign Amount', 'Total Received', 'Other Due', 'Total Due', 'Desk', 'Status', 'Placement Date', 'Last Work Date']

# Writing header.
writeHeader(closed, 0, 0, headers, format=f_headers_format)

row = 1

cursor.execute(SQLCommand_closed % client_id) 

results = cursor.fetchone() 

while results:
	closed.write(row, 0, str(results[0]), f_text_left_tahoma_8)
	closed.write(row, 1, str(results[1]), f_text_left_tahoma_8)
	closed.write(row, 2, str(results[2]), f_text_center_tahoma_8)
	closed.write(row, 3, str(results[3]), f_text_center_tahoma_8)
	closed.write(row, 4, str(results[4]), f_text_left_tahoma_8)
	closed.write(row, 5, str(results[5]), f_text_left_tahoma_8)
	closed.write(row, 6, str(results[6]), f_text_left_tahoma_8)
	closed.write(row, 7, str(results[7]), f_text_left_tahoma_8)
	closed.write(row, 8, str(results[8]), f_text_center_tahoma_8)
	closed.write(row, 9, str(results[9]), f_text_center_tahoma_8)
	closed.write(row, 10, str(results[10]), f_text_center_tahoma_8)
	closed.write(row, 11, str(results[11]), f_text_center_tahoma_8)

	closed_number_of_accounts += 1

	closed_total_assign_amt += float(results[4])

	closed_total_collected_amt += float(results[5])

	closed_current_balance += float(results[7])


	row += 1

	results = cursor.fetchone() 

#----------------------------------------------------------------------------

# Set column width.
pending.set_column(0, 0, 15)
pending.set_column(1, 1, 15)
pending.set_column(2, 2, 10)
pending.set_column(3, 4, 20)
pending.set_column(4, 4, 16)
pending.set_column(5, 5, 16)
pending.set_column(6, 6, 10)
pending.set_column(7, 7, 10)
pending.set_column(8, 7, 8)
pending.set_column(9, 8, 8)
pending.set_column(10, 10, 16)
pending.set_column(11, 11, 16)

# Header list.
headers = ['Account Number', 'Client Reference', 'Client ID', 'Name', 'Assign Amount', 'Total Received', 'Other Due', 'Total Due', 'Desk', 'Status', 'Placement Date', 'Last Work Date']

# Writing header.
writeHeader(pending, 0, 0, headers, format=f_headers_format)

row = 1

cursor.execute(SQLCommand_pending % client_id) 

results = cursor.fetchone() 

while results:
	pending.write(row, 0, str(results[0]), f_text_left_tahoma_8)
	pending.write(row, 1, str(results[1]), f_text_left_tahoma_8)
	pending.write(row, 2, str(results[2]), f_text_center_tahoma_8)
	pending.write(row, 3, str(results[3]), f_text_center_tahoma_8)
	pending.write(row, 4, str(results[4]), f_text_left_tahoma_8)
	pending.write(row, 5, str(results[5]), f_text_left_tahoma_8)
	pending.write(row, 6, str(results[6]), f_text_left_tahoma_8)
	pending.write(row, 7, str(results[7]), f_text_left_tahoma_8)
	pending.write(row, 8, str(results[8]), f_text_center_tahoma_8)
	pending.write(row, 9, str(results[9]), f_text_center_tahoma_8)
	pending.write(row, 10, str(results[10]), f_text_center_tahoma_8)
	pending.write(row, 11, str(results[11]), f_text_center_tahoma_8)

	pending_number_of_accounts += 1

	pending_total_assign_amt += float(results[4])

	pending_total_collected_amt += float(results[5])

	pending_current_balance += float(results[7])

	row += 1

	results = cursor.fetchone() 

#----------------------------------------------------------------------------

# Set column width.
hold.set_column(0, 0, 15)
hold.set_column(1, 1, 15)
hold.set_column(2, 2, 10)
hold.set_column(3, 4, 20)
hold.set_column(4, 4, 16)
hold.set_column(5, 5, 16)
hold.set_column(6, 6, 10)
hold.set_column(7, 7, 10)
hold.set_column(8, 7, 8)
hold.set_column(9, 8, 8)
hold.set_column(10, 10, 16)
hold.set_column(11, 11, 16)

# Header list.
headers = ['Account Number', 'Client Reference', 'Client ID', 'Name', 'Assign Amount', 'Total Received', 'Other Due', 'Total Due', 'Desk', 'Status', 'Placement Date', 'Last Work Date']

# Writing header.
writeHeader(hold, 0, 0, headers, format=f_headers_format)

row = 1

cursor.execute(SQLCommand_hold % client_id) 

results = cursor.fetchone() 

while results:
	hold.write(row, 0, str(results[0]), f_text_left_tahoma_8)
	hold.write(row, 1, str(results[1]), f_text_left_tahoma_8)
	hold.write(row, 2, str(results[2]), f_text_center_tahoma_8)
	hold.write(row, 3, str(results[3]), f_text_center_tahoma_8)
	hold.write(row, 4, str(results[4]), f_text_left_tahoma_8)
	hold.write(row, 5, str(results[5]), f_text_left_tahoma_8)
	hold.write(row, 6, str(results[6]), f_text_left_tahoma_8)
	hold.write(row, 7, str(results[7]), f_text_left_tahoma_8)
	hold.write(row, 8, str(results[8]), f_text_center_tahoma_8)
	hold.write(row, 9, str(results[9]), f_text_center_tahoma_8)
	hold.write(row, 10, str(results[10]), f_text_center_tahoma_8)
	hold.write(row, 11, str(results[11]), f_text_center_tahoma_8)

	hold_number_of_accounts += 1

	hold_total_assign_amt += float(results[4])

	hold_total_collected_amt += float(results[5])

	hold_current_balance += float(results[7])

	row += 1

	results = cursor.fetchone() 

#---------------------------------------------------------------
connection.close()

# Set column width.
summary.set_column(0, 0, 7)
summary.set_column(1, 1, 15)

summary.set_column(2, 2, 20)
summary.set_column(3, 4, 20)
summary.set_column(4, 4, 20)
summary.set_column(5, 5, 20)
summary.set_column(6, 6, 20)

# Header list.
headers1 = ['Number of Accounts', 'Total Assign Amount', 'Total Collected Amount', 'Current Balance', 'Collection Recovery %']

# Writing header.
writeHeader(summary, 2, 2, headers1, format=f_headers_format)

summary.write(3, 1, 'ACTIVE', f_headers_format)
summary.write(4, 1, 'CLOSED', f_headers_format)
summary.write(5, 1, 'PENDING', f_headers_format)
summary.write(6, 1, 'HOLD', f_headers_format)

summary.write(8, 1, 'Total', f_headers_format)

total_number_of_accounts = active_number_of_accounts + closed_number_of_accounts + pending_number_of_accounts + hold_number_of_accounts
total_assign_amt = active_total_assign_amt + closed_total_assign_amt + pending_total_assign_amt + hold_total_assign_amt
total_collected_amt = active_total_collected_amt + closed_total_collected_amt + pending_total_collected_amt + hold_total_collected_amt
total_current_balance = active_current_balance + closed_current_balance + pending_current_balance + hold_current_balance

summary.write(3, 2, active_number_of_accounts, f_text_left_tahoma_8)
summary.write(4, 2, closed_number_of_accounts, f_text_left_tahoma_8)
summary.write(5, 2, pending_number_of_accounts, f_text_left_tahoma_8)
summary.write(6, 2, hold_number_of_accounts, f_text_left_tahoma_8)

summary.write(8, 2, total_number_of_accounts, f_text_left_tahoma_8)

summary.write(3, 3, active_total_assign_amt, f_text_left_tahoma_8)
summary.write(4, 3, closed_total_assign_amt, f_text_left_tahoma_8)
summary.write(5, 3, pending_total_assign_amt, f_text_left_tahoma_8)
summary.write(6, 3, hold_total_assign_amt, f_text_left_tahoma_8)

summary.write(8, 3, total_assign_amt, f_text_left_tahoma_8)

summary.write(3, 4, active_total_collected_amt, f_text_left_tahoma_8)
summary.write(4, 4, closed_total_collected_amt, f_text_left_tahoma_8)
summary.write(5, 4, pending_total_collected_amt, f_text_left_tahoma_8)
summary.write(6, 4, hold_total_collected_amt, f_text_left_tahoma_8)

summary.write(8, 4, total_collected_amt, f_text_left_tahoma_8)

summary.write(3, 5, active_current_balance, f_text_left_tahoma_8)
summary.write(4, 5, closed_current_balance, f_text_left_tahoma_8)
summary.write(5, 5, pending_current_balance, f_text_left_tahoma_8)
summary.write(6, 5, hold_current_balance, f_text_left_tahoma_8)

summary.write(8, 5, total_current_balance, f_text_left_tahoma_8)

if active_total_assign_amt == 0:
	active_recovery_percent = 0.0
else:
	active_recovery_percent = active_total_collected_amt / active_total_assign_amt * 100

if closed_total_assign_amt == 0:
	closed_recovery_percent = 0.0
else:
	closed_recovery_percent = closed_total_collected_amt / closed_total_assign_amt * 100

if pending_total_assign_amt == 0:
	pending_recovery_percent = 0.0
else:
	pending_recovery_percent = pending_total_collected_amt / pending_total_assign_amt * 100

if hold_total_assign_amt == 0:
	hold_recovery_percent = 0.0
else:
	hold_recovery_percent = hold_total_collected_amt / hold_total_assign_amt * 100

if total_assign_amt == 0:
	total_recovery_percent = 0.0
else:
	total_recovery_percent = total_collected_amt / total_assign_amt * 100

summary.write(3, 6, str(round(active_recovery_percent ,2)) + '%', f_text_left_tahoma_8)
summary.write(4, 6, str(round(closed_recovery_percent ,2)) + '%', f_text_left_tahoma_8)
summary.write(5, 6, str(round(pending_recovery_percent ,2)) + '%', f_text_left_tahoma_8)
summary.write(6, 6, str(round(hold_recovery_percent ,2)) + '%', f_text_left_tahoma_8)

summary.write(8, 6, str(round(total_recovery_percent ,2)) + '%', f_text_left_tahoma_8)

try:
	h11 = float(active_number_of_accounts)/float(total_number_of_accounts)
except ZeroDivisionError:
	h11 = 0

try:
	h12 = float(closed_number_of_accounts)/float(total_number_of_accounts)
except ZeroDivisionError:
	h12 = 0

try:
	h13 = float(pending_number_of_accounts)/float(total_number_of_accounts)
except ZeroDivisionError:
	h13 = 0

try:
	h14 = float(hold_number_of_accounts)/float(total_number_of_accounts)
except ZeroDivisionError:
	h14 = 0

try:
	i11 = float(active_total_assign_amt)/float(total_assign_amt)
except ZeroDivisionError:
	i11 = 0

try:
	i12 = float(closed_total_assign_amt)/float(total_assign_amt)
except ZeroDivisionError:
	i12 = 0

try:
	i13 = float(pending_total_assign_amt)/float(total_assign_amt)
except ZeroDivisionError:
	i13 = 0

try:
	i14 = float(hold_total_assign_amt)/float(total_assign_amt)
except ZeroDivisionError:
	i14 = 0

summary.write('C20', h11 , f_percent)
summary.write('C21', h12 , f_percent)
summary.write('C22', h13 , f_percent)
summary.write('C23', h14 , f_percent)

summary.write('F20', i11 , f_percent)
summary.write('F21', i12 , f_percent)
summary.write('F22', i13 , f_percent)
summary.write('F23', i14 , f_percent)


# Creates the units chart as type pie.
units_chart = workbook.add_chart({'type': 'pie'})

# Setting series and customer colors.
units_chart.add_series({
    'name': 'Accounts Placed',
    'categories': ['Summary', 3, 1, 6, 1],
    'values': ['Summary', 19, 2, 23, 2],
    'data_labels': {'value': True , 'leader_lines': True},
    'data_labels': {'percentage': True},
    'points': [
        {'fill': {'color': '#8989E4'}},
        {'fill': {'color': '#892E5B'}},
        {'fill': {'color': '#F0F0C0'}},
        {'fill': {'color': '#B6E4E4'}},
    ],

})

# Adds black border to units_chart.
units_chart.set_chartarea({
    'border': {'color': 'black'},

})
# Add a title.
units_chart.set_title({'name': 'Accounts Placed' , 'name_font': {'name': 'Arial', 'size' : 9, 'color': '#0054AC', 'position' : 'center'},})

units_chart.set_rotation(90)

# Insert the chart into the worksheet.
summary.insert_chart('B12', units_chart , {'x_scale': 0.65, 'y_scale': 1.18})

# Creates the amount chart as type pie.
amount_chart = workbook.add_chart({'type': 'pie'})

# Setting series and customer colors.
amount_chart.add_series({
    'name': 'Amounts Placed',
    'categories': ['Summary', 3, 1, 6, 1],
    'values': ['Summary', 19, 5, 23, 5],
    'data_labels': {'value': True , 'leader_lines': True},
    'data_labels': {'percentage': True},
	'points': [
        {'fill': {'color': '#8989E4'}},
        {'fill': {'color': '#892E5B'}},
        {'fill': {'color': '#F0F0C0'}},
        {'fill': {'color': '#B6E4E4'}},
    ],
})

# Adds black border to amount_chart.
amount_chart.set_chartarea({
 	'border': {'color': 'black'},
 })

# Add a title.
amount_chart.set_title({'name': 'Amounts Placed $', 'name_font': {'name': 'Arial', 'size' : 9, 'color': '#0054AC', 'position' : 'center'},})

amount_chart.set_rotation(90)

# Insert the chart into the worksheet (with an offset).
summary.insert_chart('E12', amount_chart , {'x_scale': 0.65, 'y_scale': 1.18})

workbook.close()