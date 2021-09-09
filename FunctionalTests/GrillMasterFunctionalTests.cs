using ApplicationCore;
using AutoMapper;
using GrillMaster;
using Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests
{
    public class GrillMasterFunctionalTests
    {
        private readonly RoundGeneratorService roundGeneratorService;
        private readonly GrillMenuRequestService requestService;
        private readonly IMapper mapper;
        private readonly List<GrillMenu> mockMenus;
        private const string BASE_URL = "http://isol-grillassessment.azurewebsites.net/api/";


        public GrillMasterFunctionalTests()
        {

            HttpClient mockClient = new HttpClient();
            HttpClientHelper.AddHttpBaseClientConfig(mockClient, BASE_URL);
            Mock<IHttpClientFactory> mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory.Setup(x => x.CreateClient(Client.DEFAULT)).Returns(mockClient);

            requestService = new GrillMenuRequestService(mockHttpClientFactory.Object);

            roundGeneratorService = new RoundGeneratorService();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new GrillProfile());
            });

            mapper = mapperConfig.CreateMapper();

            mockMenus = GrillMenuMockHelper.MockMenus();
        }

        [Fact]
        public async Task RequestDataFromMenuRequestService()
        {
            IReadOnlyList<GrillMenu> data = await requestService.GetAllAsync();
            Assert.False(data.IsNullOrEmpty());
            Assert.False(data.SelectMany(x => x.Items).IsNullOrEmpty());
        }

        [Fact]
        public void MapDataFromMockedMenus()
        {
            var mappedObject = mapper.Map<List<GrillMenuModel>>(mockMenus);
            Assert.True(mappedObject != null);
            Assert.True(mappedObject.Count == mockMenus.Count);
            Assert.True(mappedObject.SelectMany(x => x.Items).Count() == mockMenus.SelectMany(x => x.Items).Count());
        }
        [Fact]
        public async Task GenerateDataFromRoundGeneratorService()
        {
            IReadOnlyList<GrillMenu> data = await requestService.GetAllAsync();
            var mappedMenus = mapper.Map<List<GrillMenuModel>>(data);
            foreach (var menu in mappedMenus)
            {
                var rounds = await roundGeneratorService.GenerateRounds(menu);
                Assert.True(rounds != null, "Round is null");
                Assert.True(rounds.Count > 0, "Round is empty ");
                Assert.True(rounds.SelectMany(x => x.GrillRoundStrips).Sum(x => x.GrillMenuItems.Count) == menu.Items.Sum(z => z.Quantity),
                    $"The number of items grilled {rounds.SelectMany(x => x.GrillRoundStrips).Sum(x => x.GrillMenuItems.Count) }!= number of menu items {menu.Items.Sum(z => z.Quantity)}");
                foreach (var round in rounds)
                {
                    Assert.True(round.GrillRoundStrips.Sum(x => x.Height) <= GrillModel.HEIGHT, "The sum of the height of all strips of a round is bigger than GrillSize");
                    Assert.True(round.GrillRoundStrips.Max(x => x.Width) <= GrillModel.WIDTH, "The max of the width of all strips of a round is bigger than GrillSize");
                }
            }
        }
        [Fact]
        public async Task GenerateDataFromRoundGeneratorServiceWRandomData()
        {
            var mappedMenus = mapper.Map<List<GrillMenuModel>>(mockMenus);
            foreach (var menu in mappedMenus)
            {
                var rounds = await roundGeneratorService.GenerateRounds(menu);
                Assert.True(rounds != null, "Round is null");
                Assert.True(rounds.Count > 0, "Round is empty ");
                Assert.True(rounds.SelectMany(x => x.GrillRoundStrips).Sum(x => x.GrillMenuItems.Count) == menu.Items.Sum(z => z.Quantity),
                    $"The number of items grilled {rounds.SelectMany(x => x.GrillRoundStrips).Sum(x => x.GrillMenuItems.Count) }!= number of menu items {menu.Items.Sum(z => z.Quantity)}");
                foreach (var round in rounds)
                {
                    Assert.True(round.GrillRoundStrips.Sum(x => x.Height) <= GrillModel.HEIGHT, "The sum of the height of all strips of a round is bigger than GrillSize");
                    Assert.True(round.GrillRoundStrips.Max(x => x.Width) <= GrillModel.WIDTH, "The max of the width of all strips of a round is bigger than GrillSize");
                }
            }
        }

        [Fact]
        public async Task GenerateDataFromRoundGeneratorServiceWLimitValues()
        {
            var mappedMenus = new List<GrillMenuModel>
            {
                new GrillMenuModel
                {
                    Menu=string.Empty,
                    Items= new List<GrillMenuItemModelWQuantity>
                    {
                        new GrillMenuItemModelWQuantity
                        {
                            Height=int.MinValue,
                            Width=int.MinValue,
                            Quantity=int.MinValue
                        },
                         new GrillMenuItemModelWQuantity
                        {
                            Height=int.MaxValue,
                            Width=int.MaxValue,
                            Quantity=int.MaxValue
                        },
                         new GrillMenuItemModelWQuantity
                        {
                            Height=0,
                            Width=0,
                            Quantity=0
                         },
                        new GrillMenuItemModelWQuantity
                        {
                            Height=100,
                            Width=5,
                            Quantity=5
                        },
                        new GrillMenuItemModelWQuantity
                        {
                            Height=5,
                            Width=100,
                            Quantity=5
                        }
                    }
                }
            };
            foreach (var menu in mappedMenus)
            {
                var rounds = await roundGeneratorService.GenerateRounds(menu);
                Assert.True(rounds != null, "Round is null");
                Assert.True(rounds.Count == 0, "Round is not empty");
                rounds = await roundGeneratorService.GenerateRounds(null);
                Assert.True(rounds != null, "Round is null");
                Assert.True(rounds.Count == 0, "Round is not empty");
                rounds = await roundGeneratorService.GenerateRounds(new GrillMenuModel());
                Assert.True(rounds != null, "Round is null");
                Assert.True(rounds.Count == 0, "Round is not empty");
            }
        }
    }
}
