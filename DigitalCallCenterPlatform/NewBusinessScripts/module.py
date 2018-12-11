import sys
import csv
import re
import pypyodbc 
import codecs
import time

connection = pypyodbc.connect("Driver={SQL Server Native Client 11.0}; Server=(localdb)\MSSQLLocalDB; Database=aspnet-DigitalCallCenterPlatform-20181031030152; Trusted_Connection=yes;") 
cursor = connection.cursor() 

def flexibleHeaderMap(header, reqs, show_extra=False):
    """ Return a mapped list of header names to column indices.
        Only found headers will be part of the return map.
    """

    # Non alphanumeric class with space and some punctuation
    pattern = re.compile('[^a-zA-Z0-9_\ \/#?-]+')
    nml_header = [pattern.sub('', x.lower()).strip() for x in header]
    # Map each required column to an index value in the header
    # Return list
    h = {}
    for col in reqs:
        if type(col) is list:
            for col_name in col:
                col_name = pattern.sub('', col_name.lower()).strip()
                if col_name in nml_header:
                    # Map the index to the first column name given regardless of which column name is actually used
                    h[col[0]] = nml_header.index(col_name)
                elif show_extra:
                    print (col)
        else:
            col = pattern.sub('', col.lower()).strip()
            if col in nml_header:
                h[col] = nml_header.index(col)
            elif show_extra:
                print (col)
    return h

def parseFile(filename):
    """ Read and parse the nb file
    """
    # Return list
    tmp_list = []
    c = 1

    reqs = [['cli_ref'],['client_id'],['name'],['desk'],
            ['prefix_1'],['phone_no_1'],['extenstion_1'],
            ['invoice_no'],['invoice_amount'],['invoice_due'],['invoice_date'],['invoice_due_date'],
            ['prefix_2'],['phone_no_2'],['extenstion_2'],
            ['full_name'],['contct'],['address'],['email'],['contry'],['timezone']]
    # Parse the trust file
    rows = csv.reader(codecs.open(filename, 'rU', 'utf-16'), delimiter=",")

    header = rows.__next__()
    h = flexibleHeaderMap(header, reqs)

    for l in rows:
        c += 1
        if len(l[h['cli_ref']]) > 1:
            # Required fields
            cli_ref = l[h['cli_ref']]
            client_id = l[h['client_id']]
            name = l[h['name']]
            desk = l[h['desk']]
            prefix_1 = l[h['prefix_1']]
            phone_no_1 = l[h['phone_no_1']]
            invoice_no = l[h['invoice_no']]
            invoice_amount = float(l[h['invoice_amount']])
            invoice_due = float(l[h['invoice_due']])
            invoice_date = l[h['invoice_date']]

            if 'extenstion_1' in h:
                extenstion_1 = l[h['extenstion_1']]
            else:
                extenstion_1 = ' '

            if 'invoice_due_date' in h:
                invoice_due_date = l[h['invoice_due_date']]
            else:
                invoice_due_date = invoice_date

            if 'prefix_2' in h:
                prefix_2 = l[h['prefix_2']]
            else:
                prefix_2 = ' '

            if 'phone_no_2' in h:
                phone_no_2 = l[h['phone_no_2']]
            else:
                phone_no_2 = ' '

            if 'extenstion_2' in h:
                extenstion_2 = l[h['extenstion_2']]
            else:
                extenstion_2 = ' '

            if 'full_name' in h:
                full_name = l[h['full_name']]
            else:
                full_name = ' '

            if 'contct' in h:
                contct = l[h['contct']]
            else:
                contct = ' '

            if 'address' in h:
                address = l[h['address']]
            else:
                address = ' '

            if 'email' in h:
                email = l[h['email']]
            else:
                email = ' '

            if 'contry' in h:
                contry = l[h['contry']]
            else:
                contry = ' '

            if 'timezone' in h:
                timezone = l[h['timezone']]
            else:
                timezone = ' '

            tmp_list.append([cli_ref, client_id, name, desk, prefix_1, phone_no_1, extenstion_1, prefix_2, phone_no_2, extenstion_2, invoice_no, invoice_amount, invoice_due, invoice_date, invoice_due_date, full_name, contct, address, email, contry, timezone])

    return c, tmp_list