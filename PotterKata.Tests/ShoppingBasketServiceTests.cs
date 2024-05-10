using Moq;
using PotterKata.Interfaces;
using PotterKata.Models.Discounts;
using PotterKata.Models.Shopping;
using PotterKata.Services;

namespace PotterKata.Tests
{
    public class ShoppingBasketServiceTests : TestsBase
    {
        public ShoppingBasketServiceTests(IDemoDataService demoDataService, IDiscountSchemeService discountSchemeService)
        {
            DemoDataService = demoDataService;
            DiscountSchemeService = discountSchemeService;

            MockDataService = new Mock<IDataService>();
            ShoppingBasketService = new ShoppingBasketService(MockDataService.Object);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenBasketIsNotInitialised_FailsAndWarnsBasketIsUnavailable()
        {
            //arrange
            ShoppingBasket shoppingBasket = null;

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(0, calcResult.DiscountedBasketTotal);
            Assert.False(calcResult.IsSuccessful);
            Assert.Equal("Basket is unavailable", calcResult.Message);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenNoDiscountsAvailable_FailsAndWarnsNoDiscountsAvailable()
        {
            //arrange
            ShoppingBasket shoppingBasket = new ShoppingBasket();

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(null as List<DiscountSchemeModel>));
            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(0, calcResult.DiscountedBasketTotal);
            Assert.False(calcResult.IsSuccessful);
            Assert.Equal("No discounts available", calcResult.Message);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenDiscountsAvailableButNotForGivenId_FailsAndWarnsUserToConfigurePromotion()
        {
            //arrange
            ShoppingBasket shoppingBasket = new ShoppingBasket();

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            var expectedResponseMessage = $"Please configure Harrys magical promotion with Id as 2";

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, 2);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(0, calcResult.DiscountedBasketTotal);
            Assert.False(calcResult.IsSuccessful);
            Assert.Equal(expectedResponseMessage, calcResult.Message);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenBasketContainsNoItems_FailsAndWarnsBasketIsEmpty()
        {
            //arrange
            ShoppingBasket shoppingBasket = new ShoppingBasket();

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(0, calcResult.DiscountedBasketTotal);
            Assert.False(calcResult.IsSuccessful);
            Assert.Equal("Basket is empty", calcResult.Message);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenBasketContainsMoreThanFiveUniqueBooks_FailsAndWarnsBasketIsEmpty()
        {
            //arrange
            ShoppingBasket shoppingBasket = await DemoDataService.GetDemoShoppingBasketDataWithSixUniqueBooks();

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(0, calcResult.DiscountedBasketTotal);
            Assert.False(calcResult.IsSuccessful);
            Assert.Equal("Discount not configured for 6 distinct books", calcResult.Message);
        }

        [Fact]
        public async Task CalculateBasketDiscount_WhenBasketContainsSingleProduct_PriceReturnedWithoutDiscount()
        {
            //arrange
            ShoppingBasket shoppingBasket = await DemoDataService.GetDemoShoppingBasketDataWithUniqueItems(1);

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(8, calcResult.DiscountedBasketTotal);
            Assert.True(calcResult.IsSuccessful);
            Assert.Equal("Success", calcResult.Message);
        }

        [Theory]
        [InlineData(2, 15.20)]
        [InlineData(3, 21.60)]
        [InlineData(4, 25.60)]
        [InlineData(5, 30.00)]
        public async Task CalculateBasketDiscount_WhenBasketContainsMoreThanOneUniqueProduct_PriceReturnedWithExpectedDiscounts(int uniqueBooks, decimal expectedBasketPrice)
        {
            //arrange
            ShoppingBasket shoppingBasket = await DemoDataService.GetDemoShoppingBasketDataWithUniqueItems(uniqueBooks);

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(expectedBasketPrice, calcResult.DiscountedBasketTotal);
            Assert.True(calcResult.IsSuccessful);
            Assert.Equal("Success", calcResult.Message);
        }

        [Theory]
        [InlineData(2, 2, 3, 3, 5, 97.6)]
        [InlineData(4, 3, 2, 5, 5, 122.4)]
        [InlineData(0, 1, 2, 3, 4, 70.4)]
        [InlineData(1, 1, 5, 5, 5, 116.40)]
        public async Task CalculateBasketDiscount_WhenBasketContainsMixtureOfBooksInBulk_PriceReturnedWithExpectedDiscounts(int b1Qty, int b2Qty, int b3Qty, int b4Qty, int b5Qty, decimal expectedBasketPrice)
        {
            //arrange
            ShoppingBasket shoppingBasket = await DemoDataService.GetDemoShoppingBasketDataWithBulkItems(b1Qty, b2Qty, b3Qty, b4Qty, b5Qty);

            MockDataService.Setup(s => s.GetAllHarrysMagicDiscountSchemes()).Returns(Task.FromResult(await DemoDataService.GetAllHarrysMagicDiscountSchemes()));

            //act
            var calcResult = await ShoppingBasketService.CalculateBasketDiscount(shoppingBasket, CurrentDiscountSchemeId);

            //assert
            Assert.NotNull(calcResult);
            Assert.Equal(expectedBasketPrice, calcResult.DiscountedBasketTotal);
            Assert.True(calcResult.IsSuccessful);
            Assert.Equal("Success", calcResult.Message);
        }
    }
}