using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BankOcr.Tests
{
    [TestClass]
    public class ReadNumbersShould
    {
        readonly BankReader bankReader = new BankReader();

        string allValue =
                "    _  _     _  _  _  _  _ " +
                "  | _| _||_||_ |_   ||_||_|" +
                "  ||_  _|  | _||_|  ||_| _|";

        [TestMethod]
        public void Return0WhenReadNumbers()
        {
            var zeroValue =
                " _  _  _  _  _  _  _  _  _ " +
                "| || || || || || || || || |" +
                "|_||_||_||_||_||_||_||_||_|";

            var result = this.bankReader.ReadNumbers(zeroValue);
            Assert.AreEqual(result, "000000000");
        }

        [TestMethod]
        public void ReturnRightNumbersWhenReadNumbers()
        {
            var result = this.bankReader.ReadNumbers(this.allValue);
            Assert.AreEqual(result, "123456789");
        }
        

        [TestMethod]
        public void ReturnAllWhenReadNumbers()
        {
            var result = this.bankReader.ReadNumbers(this.allValue);
            Assert.AreEqual(Convert.ToInt32(result), 123456789);
        }

        [TestMethod]
        public void HaveCorrectChecksum()
        {
            var entry = 345882865;

            Assert.IsTrue(this.bankReader.CheckAccount(Convert.ToInt32(entry)));
        }

        [TestMethod]
        public void HasIncorrectNumbers()
        {
            string entry =
               "    _  _     _     _  _    " +
               "  | _| _||_||_ |_   ||_||_|" +
               "  ||_  _|  | _||_|  ||_| _|";

            Assert.AreEqual("12345?78? ILL", this.bankReader.ReadNumbers(entry));
        }

        [TestMethod]
        public void HasWrongCheckSum()
        {
            string entry =
                "    _  _     _  _  _  _    " +
                "  | _| _||_||_ |_   ||_||_|" +
                "  ||_  _|  | _||_|  ||_|  |";
            var result = this.bankReader.ReadNumbers(entry);
            Assert.AreEqual("123456784 ERR", result);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CheckAccountHas9Numbers()
        {
            var nineValue =
                " _ " +
                "|_|" +
                " _|";

            this.bankReader.ReadNumbers(nineValue);
        }

        [TestMethod]
        public void FindIncorrectValueIn111111111()
        {
            var entry =
                "                           " +
                "  |  |  |  |  |  |  |  |  |" +
                "  |  |  |  |  |  |  |  |  |";

            Assert.AreEqual("711111111", this.bankReader.ReadNumbers(entry));
        }
    }
}
