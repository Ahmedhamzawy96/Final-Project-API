﻿namespace API_and_DataBase.Structures
{
        public enum TransType
        {
            Paid = 0 ,
            Get = 1 ,

        }

        public enum AccountType
        {
            Customer = 0,
            Supplier = 1,
            Car = 2,

        }
        public enum Operation
        {
            Expense = 0,
            ImportReciept = 1,
            ExportReciept = 2,
            CarTrans = 3, 
            SuppplierTrans = 4,
            CustomerTrans = 5,

        }
    }
