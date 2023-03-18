using Microsoft.VisualStudio.TestTools.UnitTesting;
using integtest;
using Moq;
using System;
using System.Collections.Generic;
using Npgsql;
using integtest.Interfaces;
using integtest.Classes;
using Microsoft.EntityFrameworkCore;

namespace Laba4Tests
{
    [TestClass]
    
    public class TriangleValidateServiceUnitTests
    {
        private Mock<ITriangleProvider> triangleProvider;
        private ITriangleService triangleService;
        private ITriangleValidateService triangleValidateService;
        //private Mock<ApplicationContext> applicationContext;
        [TestInitialize]
        public void TestInitialize()
        {
            triangleProvider = new Mock<ITriangleProvider>();
            triangleService = new TriangleService();
            //applicationContext = new Mock<ApplicationContext>();
            triangleValidateService = new TriangleValidateService(triangleProvider.Object, triangleService /*applicationContext.Object*/);
        }

        [TestMethod]
        public void IsValid_True()
        {
            triangleProvider.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Triangle(1, 7, 7, 7, 21.21762239271875, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            Assert.AreEqual(true, triangleValidateService.IsValid(1));
        }
        [TestMethod]
        public void IsValid_False()
        {
            triangleProvider.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Triangle(2, -7, 7, 7, 0.0027712812921102037, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            Assert.AreEqual(false, triangleValidateService.IsValid(2));
        }
        [TestMethod]
        public void IsValid_EmptyTriangle()
        {
            triangleProvider.Setup(m => m.GetById(It.IsAny<int>())).Returns(new Triangle());
            Assert.AreEqual(false, triangleValidateService.IsValid(4));

        }
        [TestMethod]
        public void IsAllValid_True()
        {
            triangleProvider.Setup(m => m.GettAll()).Returns(new List<Triangle> { new Triangle(3, 7, 7, 7, 21.21762239271875, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon), new Triangle(4, 7, 7, 7, 21.21762239271875, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon) });
            Assert.AreEqual(true, triangleValidateService.IsAllValid());
        }
        [TestMethod]
        public void IsAllValid_False()
        {
            triangleProvider.Setup(m => m.GettAll()).Returns(new List<Triangle> { new Triangle(5, -7, 7, 7, 0.0027712812921102037, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon), new Triangle(6, -7, 7, 7, 0.0027712812921102037, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon) });
            Assert.AreEqual(false, triangleValidateService.IsAllValid());

        }
        [TestMethod]
        public void IsAllValid_EmptyTriangle()
        {
            triangleProvider.Setup(m => m.GettAll()).Returns(new List<Triangle> { new Triangle(), new Triangle() });
            Assert.AreEqual(false, triangleValidateService.IsAllValid());
        }
    }
    [TestClass]
    public class TriangleValidateServiceIntegrationTests
    {
        private ITriangleProvider triangleProvider;
        private ITriangleService triangleService;
        private ApplicationContext applicationContext;
        private ITriangleValidateService triangleValidateService;
       

        [TestInitialize]
        public void TestInitialize()
        {
            triangleProvider = new TriangleProvider();
            triangleService = new TriangleService();
            applicationContext = new ApplicationContext();
            triangleValidateService = new TriangleValidateService(triangleProvider, triangleService, applicationContext);
        }
        [TestMethod]
        public void IsValidTrue()
        {
            triangleProvider.Save(new Triangle(1, 9, 9, 9, 35.074028853269764, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            bool actual = triangleValidateService.IsValid(1);
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IsAllValid_false()
        {
            triangleProvider.Save(new Triangle(2, -9, 9, 9, 35.074028853269764, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            bool actual = triangleValidateService.IsAllValid();
            Assert.AreEqual(false, actual);
        }
        
        [TestMethod]
        public void GetTypeTriangle()
        {
            Triangle triangle;
            triangleProvider.Save(triangle = new Triangle(3, 9, 9, 9, 35.074028853269764, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            Triangle.TriangleType expected = triangle.type;
            Triangle.TriangleType actual = triangleService.GetType(triangle.a, triangle.b, triangle.c);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetAreaTriangle()
        {
            Triangle triangle;
            triangleProvider.Save(triangle = new Triangle(4, 9, 9, 9, 35.074028853269764, Triangle.TriangleType.Equilateral | Triangle.TriangleType.Oxygon));
            double expected = triangle.area;
            double actual = triangleService.GetArea(triangle.a, triangle.b, triangle.c);
            Assert.IsTrue(Math.Abs(expected - actual) < 1e-9);
        }
        [TestMethod]
        public void IsValidTriangle()
        {
            triangleProvider.Save(new Triangle(5, 10, 10, 1, 4.993746088859544, Triangle.TriangleType.Isosceles | Triangle.TriangleType.Oxygon));
            Assert.IsTrue(triangleValidateService.IsValid(5));
        }
    }
}
