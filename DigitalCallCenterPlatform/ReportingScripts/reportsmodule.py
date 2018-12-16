import sys, getopt
import pypyodbc 
import xlsxwriter
from datetime import *

connection = pypyodbc.connect("Driver={SQL Server Native Client 11.0}; Server=(localdb)\MSSQLLocalDB; Database=aspnet-DigitalCallCenterPlatform-20181031030152; Trusted_Connection=yes;") 
cursor = connection.cursor() 

# Loop through a write headers at a specific row with an optional format
def writeHeader(ws, row, col, header_list, format=None):
	offset = 0
	for header in header_list:
		if format:
			ws.write(row, col+offset, header, format)
		else:
			ws.write(row, col+offset, header)
		offset += 1

# Define format properties here...
text_center_tahoma_8 = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'center',
    'border' : 1
}
text_left_tahoma_8 = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'left',
    'border' : 1
}
header_props = {
	'bold': True,
	'bottom': True,
	'bg_color': '#87CEFA',
	'font_size': 10
}
header_no_underline_props = {
	'bold': True,
	'bg_color': '#87CEFA',
	'font_size': 10
}
title_props = {
	'bold': True,
	'color': '#005287',
	'font_size': 20,
	'align': 'right'
}
title_desc_props = {
	'bold': True,
	'italic': True,
	'color': '#005287',
	'font_size': 10,
	'align': 'right'
}
subtotal_props = {
	'bold': True,
	'top': True,
	'num_format': '#,##0.00',
	'font_size': 10
}
subtotal_red_props = {
	'bold': True,
	'top': True,
	'num_format': '#,##0.00',
	'font_size': 10,
	'color': 'red'
}
default_bold_props = {
	'bold': True,
	'font_size': 10,
	'align': 'center'
}
default_left_bold_props = {
	'bold': True,
	'font_size': 10,
	'align': 'left'
}
default_props = {
	'font_size': 10,
	'align': 'center'
}
default_gray_props = {
	'font_size': 10,
	'align': 'center',
	'bg_color': '#D3D3D3'
}
default_left_props = {
	'font_size': 10,
	'align': 'left'
}
default_left_gray_props = {
	'font_size': 10,
	'align': 'left',
	'bg_color': '#D3D3D3'
}
numeric_props = {
	'num_format': '#,##0.00',
	'font_size': 10
}
numeric_gray_props = {
	'num_format': '#,##0.00',
	'font_size': 10,
	'bg_color': '#D3D3D3'
}
integer_props = {
	'num_format': '#,##0',
	'font_size': 10
}
integer_gray_props = {
	'num_format': '#,##0',
	'font_size': 10,
	'bg_color': '#D3D3D3'
}
percent_props = {
	'num_format': '##0.00%',
	'font_size': 10
}
percent_gray_props = {
	'num_format': '##0.00%',
	'font_size': 10,
	'bg_color': '#D3D3D3'
}
fx_props = {
	'num_format': '0.00000',
	'font_size': 10
}
fx_gray_props = {
	'num_format': '0.00000',
	'font_size': 10,
	'bg_color': '#D3D3D3'
}
date_props = {
	'num_format': 'yyyy-mm-dd',
	'font_size': 10,
	'align': 'center'
}
date_gray_props = {
	'num_format': 'yyyy-mm-dd',
	'font_size': 10,
	'bg_color': '#D3D3D3',
	'align': 'center'
}
link_props = {
	'color': 'blue',
	'underline': 1,
	'font_size': 10
}
link_gray_props = {
	'color': 'blue',
	'underline': 1,
	'font_size': 10,
	'bg_color': '#D3D3D3'
}

# Format used in General Electric and Dupont reports.
text_warp_left_format = {
    'align': 'left',
    'valign': 'top',
    'text_wrap': True,
    'border': 1,
    'font_size': 8
}

headers_format = {
    'bold' : True,
    'bg_color' : '#D3D3D3',
    'font_size' : 8,
    'align' : 'center',
    'font_name' : 'Tahoma',
    'font_color' : '#004C81',
    'border' : 1,
}

text_left = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'left',
    'border' : 1
}

text_left_gray = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'left',
    'bg_color': '#D3D3D3',
    'border' : 1
}

text_right = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'border' : 1
}

text_center = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'center',
    'border' : 1
}

text_right_gray = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'bg_color': '#D3D3D3',
    'border' : 1
}

date = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'center',
    'num_format': 'MM-DD-YYYY',
    'border' : 1
}

date_gray = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'center',
    'num_format': 'MM-DD-YYYY',
    'bg_color': '#D3D3D3',
    'border' : 1
}

amount_right = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'num_format': '$#,##0.00',
    'border' : 1,
    'num_format': '$#,##0.00'
}

amount_right_gray = {
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'num_format': '$#,##0.00',
    'bg_color': '#D3D3D3',
    'border' : 1
}


text_blue_left_bg = {
    'bold' : True,
    'bg_color' : '#D3D3D3',
    'font_size' : 8,
    'align' : 'left',
    'font_name' : 'Tahoma',
    'font_color' : '#004C81',
    'border' : 1
}

amount_blue_right_bg = {
    'bold' : True,
    'bg_color' : '#D3D3D3',
    'font_size' : 8,
    'align' : 'right',
    'font_name' : 'Tahoma',
    'border' : 1,
    'num_format': '$#,##0.00_);[RED]($#,##0.00)',
    'font_color' : '#004C81'
}

# Styles used in statements-batch-dupont and statements-batch-coke
title20 = {
    'bold' : True,
    'font_size' : 20,
    'align' : 'right',
    'font_name' : 'Arial',
    'font_color' : '#004C81'
}

title13 = {
    'bold' : True,
    'font_size' : 13,
    'align' : 'right',
    'font_name' : 'Arial',
    'font_color' : '#004C81'
}

title10 = {
    'bold' : True,
    'font_size' : 10,
    'align' : 'right',
    'font_name' : 'Arial',
    'font_color' : '#004C81'
}

text_left_bold = {
	'bold' : True,
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'left'
}

text_right_bold = {
    'bold' : True,
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right'
}

amount_right_bold = {
	'bold' : True,
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'num_format': '$##,###,###.00'
}

amount_right_red_bold = {
	'bold' : True,
    'font_size' : 8,
    'font_name' : 'Tahoma',
    'align': 'right',
    'font_color' : '#FF0000',
    'num_format': '$#,##0.00'
}


red_if_negative = {
    'font_size' : 8,
    'border' : 1,
    'font_name' : 'Tahoma',
    'align': 'right',
    'num_format': '$#,##0.00_);[RED]($#,##0.00)'
}

percent = {
    'font_size' : 8,
    'border' : 1,
    'font_name' : 'Tahoma',
    'align': 'center',
    'num_format': '0%'
}

title20_left = {
    'bold' : True,
    'font_size' : 20,
    'align' : 'left',
    'font_name' : 'Arial',
    'font_color' : '#004C81'
}

title_left_format = {
    'bold' : True,
    'bg_color' : '#D3D3D3',
    'font_size' : 8,
    'align' : 'left',
    'font_name' : 'Tahoma',
    'font_color' : '#004C81',
    'border' : 1
}