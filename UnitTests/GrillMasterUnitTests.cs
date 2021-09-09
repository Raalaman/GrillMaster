using ApplicationCore;
using AutoMapper;
using Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{

    public class GrillMasterUnitTests
    {
        private readonly Mock<IAsyncRoundGenerator<GrillMenuModel, GrillRoundModel>> mockRoundGeneratorService;
        private readonly Mock<IGrillMenuRequestService> mockRequestService;
        private readonly Mock<IMapper> mockMapper;
        private readonly List<GrillMenu> mockMenus;

        public GrillMasterUnitTests()
        {

            mockRoundGeneratorService = new Mock<IAsyncRoundGenerator<GrillMenuModel, GrillRoundModel>>();
            mockRequestService = new Mock<IGrillMenuRequestService>();


            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<GrillMenu, GrillMenuModel>(It.IsAny<GrillMenu>())).Returns(new GrillMenuModel());


            mockMenus = GrillMenuMockHelper.MockMenus();

            mockRequestService.Setup(c => c.GetAllAsync()).Returns(Task.FromResult(mockMenus as IReadOnlyList<GrillMenu>));
            mockRoundGeneratorService.Setup(x => x.GenerateRounds(It.IsAny<GrillMenuModel>()))
                                     .Returns(Task.FromResult(new List<GrillRoundModel>()));

        }

        [Fact]
        public async Task RequestDataFromMenuRequestService()
        {
            IReadOnlyList<GrillMenu> data = await mockRequestService.Object.GetAllAsync();
            Assert.False(data.IsNullOrEmpty());
            Assert.False(data.SelectMany(x => x.Items).IsNullOrEmpty());
        }

        [Fact]
        public void MapDataFromMockedMenus()
        {
            var mappedObject = mockMapper.Object.Map<GrillMenu, GrillMenuModel>(mockMenus.First());
            Assert.True(mappedObject != null);

        }
        [Fact]
        public async Task GenerateDataFromRoundGeneratorService()
        {
            var rounds = await mockRoundGeneratorService.Object.GenerateRounds(mockMapper.Object.Map<GrillMenu, GrillMenuModel>(mockMenus.First()));
            Assert.True(rounds != null);
        }
    }
}
