from module import *

""" Global sql string definitions
"""

sql_insert_account = """
INSERT INTO [dbo].[WorkPlatformModels] ([ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], [PalacementDate], [LastWorkDate]) 
VALUES ('%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s', '%s')
"""

sql_get_account_number = """
SELECT Id, AssignAmount FROM WorkPlatformModels WHERE ClientReference = '%s' AND ClientID = '%s' AND Name = '%s' AND Status = 'NEW'
"""

sql_insert_invoice = """
INSERT INTO [dbo].[InvoiceModels] ([AccountNumber], [Invoice], [Status], [Amount], [Due], [InvoiceDate], [DueDate], [PaymentRequestFlag], [PostedFlag]) VALUES ('%s', '%s', 'OPEN','%s', '%s','%s', '%s', '0', '0')
"""

sql_insert_phone = """
INSERT INTO [dbo].[PhoneModels] ([AccountNumber], [Prefix], [PhoneNumber], [Extension]) VALUES ('%s', '%s', '%s', '%s')
"""

sql_insert_address = """
INSERT INTO [dbo].[AddressModels] ([AccountNumber], [FullName], [Contact], [Address], [Email], [Country], [TimeZone]) VALUES ('%s', '%s', '%s', '%s','%s', '%s', '%s')
"""

sql_insert_note = """
INSERT INTO [dbo].[NotesModels] ([AccountNumber], [SeqNumber], [UserCode], [ActionCode], [Status], [Desk], [Note], [NoteDate]) VALUES ('%s', '%s', '%s', '%s','%s', '%s', '%s', '%s')
"""

sql_update_account = """
UPDATE WorkPlatformModels SET  AssignAmount = '%s', TotalDue = '%s' WHERE Id = '%s'
"""

if __name__ == '__main__':

	try:
		totalAmountProcessed = 0

		import datetime
		dt = datetime.datetime.today()
		now = dt.strftime("%Y-%m-%d %H:%M:%S")

		if len(sys.argv) > 1:
			file = sys.argv[1]

			file_inventory = []

			c, trust_list = parseFile(file)

			for elem in trust_list:
				# [cli_ref, client_id, name, desk, prefix_1, phone_no_1, extenstion_1, prefix_2, phone_no_2, extenstion_2, invoice_no, invoice_amount, invoice_due, invoice_date, invoice_due_date, full_name, contct, address, email, contry, timezone]
				if elem[0] not in file_inventory:
					#[ClientReference], [ClientID], [Name], [AssignAmount], [TotalReceived], [OtherDue], [TotalDue], [Desk], [Status], [PalacementDate], [LastWorkDate]
					cursor.execute(sql_insert_account % (elem[0], elem[1], elem[2], 0, 0, 0, 0, elem[3], 'NEW', now, now)) 

					connection.commit()

					file_inventory.append(elem[0])

					cursor.execute(sql_get_account_number % (elem[0], elem[1], elem[2])) 

					results = cursor.fetchone() 

					acc_number =  results[0]
					assign_amount = results[1]
					
					#[AccountNumber], [Prefix], [PhoneNumber], [Extension]
					cursor.execute(sql_insert_phone % (acc_number, elem[4], elem[5], elem[6])) 
					cursor.execute(sql_insert_phone % (acc_number, elem[7], elem[8], elem[9])) 

					#[AccountNumber], [FullName], [Contact], [Address], [Email], [Country], [TimeZone]
					cursor.execute(sql_insert_address % (acc_number, elem[15], elem[16], elem[17], elem[18], elem[19], elem[20])) 

					#[AccountNumber], [SeqNumber], [UserCode], [ActionCode], [Status], [Desk], [Note], [NoteDate]
					cursor.execute(sql_insert_note % (acc_number, 1, 'SYS', 'NEW', 'NEW', elem[3], 'System: New account placed!',now)) 

					connection.commit()

				cursor.execute(sql_get_account_number % (elem[0], elem[1], elem[2])) 

				results = cursor.fetchone() 

				acc_number =  results[0]
				assign_amount = results[1]

				#[AccountNumber], [Invoice], [Status], [Amount], [Due], [InvoiceDate], [DueDate]
				cursor.execute(sql_insert_invoice % (acc_number, elem[10], elem[11], elem[12], elem[13], elem[14])) 

				totalAmountProcessed += elem[12]

				new_assign_amount = assign_amount + elem[12]

				cursor.execute(sql_update_account % (new_assign_amount, new_assign_amount, acc_number)) 

				connection.commit()

			print(c - 1, totalAmountProcessed)

			connection.close()

	except:
		connection.rollback()
		print ("Exception: ", sys.exc_info()[0])
		raise