import sys
import csv
import re
import pypyodbc 
import codecs

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
    c = 0

    reqs = [['acc_number'],['inv_number'],['amount']]
    # Parse the trust file
    rows = csv.reader(codecs.open(filename, 'rU', 'utf-16'), delimiter=",")

    header = rows.__next__()
    h = flexibleHeaderMap(header, reqs)

    for l in rows:
        c += 1
        if len(l[h['acc_number']]) > 0:
            # Required fields
            acc_number = l[h['acc_number']]
            inv_number = l[h['inv_number']]
            amount = float(l[h['amount']])

            tmp_list.append([acc_number, inv_number, amount])

    return c, tmp_list
