using Bunit;
using SportsStore.Components;
using Xunit;

namespace SportsStore.Tests
{
    public class ComponentTests : BunitContext
    {
        [Fact]
        public void Pager_Renders_Correct_Page_Links()
        {
            // Arrange
            var cut = Render<Pager>(parameters => parameters
                .Add(p => p.CurrentPage, 2)
                .Add(p => p.TotalPages, 3)
                .Add(p => p.PageUrl, page => $"/page-{page}")
            );

            // Act
            var links = cut.FindAll("a");

            // Assert
            Assert.Equal(5, links.Count); // Previous, 1, 2, 3, Next
            Assert.Equal("/page-1", links[0].GetAttribute("href")); // Previous points to 1
            Assert.Equal("/page-2", cut.Find(".active a").GetAttribute("href")); // Active page is 2
            Assert.Equal("/page-3", links[4].GetAttribute("href")); // Next points to 3
        }
    }
}
