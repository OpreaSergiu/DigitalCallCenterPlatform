from module import *

""" Global sql string definitions
"""

sql_select_account_invoice_info = """
SELECT TotalDue, Due, AssignAmount
FROM WorkPlatformModels 
INNER JOIN InvoiceModels ON InvoiceModels.AccountNumber = WorkPlatformModels.Id
WHERE WorkPlatformModels.Id = '%s' AND InvoiceModels.Invoice = '%s'
"""

sql_update_invoice = """
UPDATE InvoiceModels SET Due = '0', Status = 'CLOSED', PaymentRequestFlag = '1', PostedFlag = '1' WHERE AccountNumber = '%s' AND Invoice = '%s'
"""

sql_update_invoice1 = """
UPDATE InvoiceModels SET Due = '%s' WHERE AccountNumber = '%s' AND Invoice = '%s'
"""

sql_update_account = """
UPDATE WorkPlatformModels SET  Status = 'CLOSED', TotalReceived = '%s', TotalDue = '0' WHERE Id = '%s'
"""

sql_update_account1 = """
UPDATE WorkPlatformModels SET TotalReceived = '%s', TotalDue = '%s' WHERE Id = '%s'
"""

if __name__ == '__main__':

	try:
		totalAmountProcessed = 0

		if len(sys.argv) > 1:
			file = sys.argv[1]

			c, trust_list = parseFile(file)

			for elem in trust_list:
				# [acc_number, inv_number, amount]
				try:
					cursor.execute(sql_select_account_invoice_info % (elem[0], elem[1])) 

					results = cursor.fetchone() 

					accountDue = float(results[0])
					invoiceDue = float(results[1])
					assignAmount = float(results[2])

					if invoiceDue != 0:
						if invoiceDue - elem[2] == 0:
							cursor.execute(sql_update_invoice % (elem[0], elem[1])) 
							if accountDue - elem[2] == 0:
								cursor.execute(sql_update_account % (assignAmount, elem[0])) 
						else:
							cursor.execute(sql_update_invoice1 % (invoiceDue - elem[2], elem[0], elem[1]))
							cursor.execute(sql_update_account1 % (assignAmount - (accountDue - elem[2]) ,accountDue - elem[2], elem[0])) 

						totalAmountProcessed += elem[2]

					connection.commit()

					print(c , totalAmountProcessed)
				except:
					connection.rollback()
					raise

			connection.close()

	except:
		connection.rollback()
		print ("Exception: ", sys.exc_info()[0])
		raise