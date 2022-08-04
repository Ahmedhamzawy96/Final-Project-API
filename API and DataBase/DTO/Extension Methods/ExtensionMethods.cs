using API_and_DataBase.Models;

namespace API_and_DataBase.DTO.Extension_Methods
{
    public static class Extensions
    {

        #region Car
        public static CarDTO CarToDTO(this Car car)
        {
            if (car == null)
            {
                return null;
            }
            else
            {
                CarDTO carDTO = new CarDTO();
                carDTO.ID = car.ID;
                carDTO.Name = car.Name;
                carDTO.Notes = car.Notes;
                carDTO.Account = car.Account;
                return carDTO;
            }

        }
        public static Car DTOToCar(this CarDTO carDto)
        {
            if (carDto == null)
            {
                return null;
            }
            else
            {
                Car car = new Car();
                car.ID = carDto.ID;
                car.Name = carDto.Name;
                car.Notes = carDto.Notes;
                car.Account = carDto.Account;
                return car;
            }
        }
        #endregion

        #region CarProduct
        public static CarProductDTO CarProductToDTO(this CarProduct carProduct)
        {

            if (carProduct == null)
            {
                return null;
            }
            else
            {
                CarProductDTO carProductDTO = new CarProductDTO();
                carProductDTO.Quantity = carProduct.Quantity;
                carProductDTO.CarID = carProduct.Car.ID;
                carProductDTO.CarName = carProduct.Car.Name;
                carProductDTO.ProductID = carProduct.Product.ID;
                carProductDTO.ProductName = carProduct.Product.Name;
                carProductDTO.SellingPrice = carProduct.Product.SellingPrice;
                return carProductDTO;
            }
        }
        public static CarProduct DTOToCarProduct(this CarProductDTO carProductDTO)
        {
            CompanyContext db =new CompanyContext();
            if (carProductDTO == null)
            {
                return null;
            }
            else
            {
                CarProduct carProduct = new CarProduct();
                 carProduct.Quantity= carProductDTO.Quantity ;
                carProduct.CarID = carProductDTO.CarID;
                carProduct.ProductID = carProductDTO.ProductID;
                carProduct.Car = db.Cars.FirstOrDefault(w=>w.ID== carProductDTO.CarID);
                carProduct.Product = db.Products.FirstOrDefault(w => w.ID == carProductDTO.ProductID);
                return carProduct;
            }
        }
        #endregion

        #region Customer
        public static CustomerDTO CustomerToDTO(this Customer customer)
        {
            if (customer == null)
            {
                return null;
            }
            else
            {
                CustomerDTO customerDTO = new CustomerDTO();
                customerDTO.ID = customer.ID;
                customerDTO.Name = customer.Name;
                customerDTO.Phone = customer.Phone;

                customerDTO.Notes = customer.Notes;
                customerDTO.Account = customer.Account;
                return customerDTO;
            }

        }
        public static Customer DTOToCustomer(this CustomerDTO customerDto)
        {
            if (customerDto == null)
            {
                return null;
            }
            else
            {
                Customer customer = new Customer();
                customer.ID = customerDto.ID;
                customer.Name = customerDto.Name;
                customer.Notes = customerDto.Notes;
                customer.Phone = customerDto.Phone;
                customer.Account = customerDto.Account;
                return customer;
            }
        }
        #endregion

        #region ExportProduct
        public static ExportProductDTO ExportProductToDTO(this ExportProduct exportProduct)
        {

            if (exportProduct == null)
            {
                return null;
            }
            else
            {
                ExportProductDTO exportProductDTO = new ExportProductDTO();
                exportProductDTO.ProductID = exportProduct.ProductID;
                exportProductDTO.ProductName = exportProduct.Product.Name;
                exportProductDTO.ProductPrice = exportProduct.Price;
                exportProductDTO.TotalPrice = exportProduct.TotalPrice;
                exportProductDTO.ExportReceiptID = exportProduct.ReceiptID;
                exportProductDTO.Quantity = exportProduct.Quantity;

                return exportProductDTO;
            }
        }
        public static ExportProduct DTOToExportProduct(this ExportProductDTO exportProductDTO)
        {
            CompanyContext db = new CompanyContext();
            if (exportProductDTO == null)
            {
                return null;
            }
            else
            {
                ExportProduct exportProduct = new ExportProduct();

                exportProduct.Quantity = exportProductDTO.Quantity;
                exportProduct.TotalPrice = exportProductDTO.TotalPrice;
                exportProduct.Price = exportProductDTO.ProductPrice;
                exportProduct.ProductID=exportProductDTO.ProductID;
                exportProduct.ReceiptID = exportProductDTO.ExportReceiptID;
                exportProduct.ExportReciept = db.ExportReciepts.FirstOrDefault(w=>w.ID==exportProductDTO.ExportReceiptID);
                exportProduct.Product = db.Products.FirstOrDefault(w => w.ID == exportProductDTO.ProductID);

                return exportProduct;
            }
        }
        #endregion

        #region Supplier
        public static SupplierDTO SupplierToDTO(this Supplier supplier)
        {
            if (supplier == null)
            {
                return null;
            }
            else
            {
                SupplierDTO supplierDTO = new SupplierDTO();
                supplierDTO.ID = supplier.ID;
                supplierDTO.Name = supplier.Name;
                supplierDTO.Phone = supplier.Phone;
                supplierDTO.Notes = supplier.Notes;
                supplierDTO.Account = supplier.Account;
                return supplierDTO;
            }

        }
        public static Supplier DTOToSupplier(this SupplierDTO supplierDTO)
        {
            if (supplierDTO == null)
            {
                return null;
            }
            else
            {
                Supplier Supplier = new Supplier();
                Supplier.ID = supplierDTO.ID;
                Supplier.Name = supplierDTO.Name;
                Supplier.Notes = supplierDTO.Notes;
                Supplier.Phone = supplierDTO.Phone;
                Supplier.Account = supplierDTO.Account;
                return Supplier;
            }
        }
        #endregion

        #region Users
        public static UsersDTO UsersToDTO(this Users users)
        {
            if (users == null)
            {
                return null;
            }
            else
            {
                UsersDTO usersDTO = new UsersDTO();
                usersDTO.UserName = users.UserName;
                usersDTO.Password = users.Password;
                usersDTO.Type = users.Type;
                usersDTO.CarID = users.Car.ID;
                usersDTO.CarName = users.Car.Name;
                return usersDTO;
            }

        }
        public static Users DTOToUsers(this UsersDTO usersDTO)
        {
            CompanyContext db = new CompanyContext();

            if (usersDTO == null)
            {
                return null;
            }
            else
            {
                Users users = new Users();
                users.UserName = usersDTO.UserName;
                users.Password = usersDTO.Password;
                users.Type = usersDTO.Type;
                users.CarID = usersDTO.CarID;
                users.Car = db.Cars.FirstOrDefault(w=>w.ID==usersDTO.CarID);
                return users;
            }
        }
        #endregion

        #region ImportProduct
        //ImportProductToDTO
        public static ImportProductDTO ImportProductToDTO(this ImportProduct importProduct)
        {
            CompanyContext db = new CompanyContext();
            if (importProduct != null)
            {
                return new ImportProductDTO
                {
                    ImportReceiptID = importProduct.ReceiptID,
                    ProductID = importProduct.ProductID,
                    ProductName = db.Products.Where(A => A.ID == importProduct.ProductID).Select(A => A.Name).FirstOrDefault(),
                    BuyinglPrice = db.Products.Where(A => A.ID == importProduct.ProductID).Select(A => A.BuyingPrice).FirstOrDefault(),
                    Quantity = importProduct.Quantity,
                    TotalPrice = importProduct.TotalPrice
                };
            }
            else
            {
                return null;
            }
        }

        //DTOToImportProduct
        public static ImportProduct DTOToImportProduct(this ImportProductDTO importProductDTO)
        {
            CompanyContext db = new CompanyContext();
            if (importProductDTO != null)
            {
                return new ImportProduct
                {
                    ReceiptID = importProductDTO.ImportReceiptID,
                    ProductID = importProductDTO.ProductID,
                    Product = db.Products.FirstOrDefault(A => A.ID == importProductDTO.ProductID),
                    Price = importProductDTO.BuyinglPrice,
                    Quantity = importProductDTO.Quantity,
                    TotalPrice = importProductDTO.TotalPrice
                };
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region ExportReciept
        //ExportRecieptToDTO
        public static ExportRecieptDTO ExportRecieptToDTO(this ExportReciept exportReciept)
        {
            CompanyContext db = new CompanyContext();

            if (exportReciept != null)
            {
                return new ExportRecieptDTO
                {
                    ID = exportReciept.ID,
                    Date = exportReciept.Date,
                    Total = exportReciept.Total,
                    Notes = exportReciept.Notes,
                    Paid = exportReciept.Paid,
                    Remaining = exportReciept.Remaining,
                    CustID = exportReciept.CustomerID,
                    CustName = db.Customers.Where(A => A.ID == exportReciept.CustomerID).Select(A => A.Name).FirstOrDefault(),
                    UserName = exportReciept.UserName,
                    CarSellID = exportReciept.CarSellID,
                    CarBuyID = exportReciept.CarBuyID
                };
            }
            else
            {
                return null;
            }
        }

        //DTOToExportReciept
        public static ExportReciept DTOToExportReciept(this ExportRecieptDTO exportRecieptDTO)
        {
            CompanyContext db = new CompanyContext();

            if (exportRecieptDTO != null)
            {
                return new ExportReciept
                {
                    ID = exportRecieptDTO.ID,
                    Date = exportRecieptDTO.Date,
                    Total = exportRecieptDTO.Total,
                    Notes = exportRecieptDTO.Notes,
                    Paid = exportRecieptDTO.Paid,
                    Remaining = exportRecieptDTO.Remaining,
                    CustomerID = exportRecieptDTO.CustID,
                    Customer = db.Customers.FirstOrDefault(A => A.ID == exportRecieptDTO.CustID),
                    UserName = exportRecieptDTO.UserName,
                    CarSellID = exportRecieptDTO.CarSellID,
                    CarBuyID = exportRecieptDTO.CarBuyID,
                    CarBuy = db.Cars.FirstOrDefault(A => A.ID == exportRecieptDTO.CarBuyID),
                    CarSell = db.Cars.FirstOrDefault(A => A.ID == exportRecieptDTO.CarSellID)
                };
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region ImportReciept
        //ImportRecieptToDTO
        public static ImportRecieptDTO ImportRecieptToDTO(this ImportReciept importReciept)
        {
            CompanyContext db = new CompanyContext();

            if (importReciept != null)
            {
                return new ImportRecieptDTO
                {
                    ID = importReciept.ID,
                    Date = importReciept.Date,
                    Total = importReciept.Total,
                    Notes = importReciept.Notes,
                    Paid = importReciept.Paid,
                    Remaining = importReciept.Remaining,
                    SUPID = importReciept.SupplierID,
                    SUPName = db.Suppliers.Where(A => A.ID == importReciept.SupplierID).Select(A => A.Name).FirstOrDefault(),
                    UserName = importReciept.UserName
                };
            }
            else
            {
                return null;
            }
        }

        //DTOToImportReciept
        public static ImportReciept DTOToImportReciept(this ImportRecieptDTO importRecieptDTO)
        {
            CompanyContext db = new CompanyContext();

            if (importRecieptDTO != null)
            {
                return new ImportReciept
                {
                    ID = importRecieptDTO.ID,
                    Date = importRecieptDTO.Date,
                    Total = importRecieptDTO.Total,
                    Notes = importRecieptDTO.Notes,
                    Paid = importRecieptDTO.Paid,
                    Remaining = importRecieptDTO.Remaining,
                    SupplierID = importRecieptDTO.SUPID,
                    Supplier = db.Suppliers.FirstOrDefault(A => A.ID == importRecieptDTO.SUPID),
                    UserName = importRecieptDTO.UserName,
                    User = db.Users.FirstOrDefault(A => A.UserName == importRecieptDTO.UserName)
                };
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Product
        //ProductToDto 
        public static ProductDTO ProductToDTO(this Product product)
        {
            if (product != null)
            {
                return new ProductDTO
                {
                    ID = product.ID,
                    Name = product.Name,
                    BuyingPrice = product.BuyingPrice,
                    SellingPrice = product.SellingPrice,
                    Quantity = product.Quantity

                };
            }
            else
            {
                return null;
            }
        }

        //DTOToProduct 
        public static Product DTOToProduct(this ProductDTO productDTO)
        {
            if (productDTO != null)
            {
                return new Product
                {
                    ID = productDTO.ID,
                    Name = productDTO.Name,
                    BuyingPrice = productDTO.BuyingPrice,
                    SellingPrice = productDTO.SellingPrice,
                    Quantity = productDTO.Quantity

                };
            }
            else
            {
                return null;
            }
        }
    }
    #endregion

}

