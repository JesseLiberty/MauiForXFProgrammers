using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ForgetMeNot.Model;
using ForgetMeNot.Services;
using ForgetMeNot.ViewModel;
using NSubstitute;

namespace ForgetMeNot.xUnitTests;

public class BuddyTests
{
    [Fact]
    public async Task InitShouldPopulateBuddies()
    {
        var buddies = new List<Buddy>();
        var buddy = new Buddy();
        buddies.Add(buddy);
        var buddyService = Substitute.For<IBuddyService>();
        buddyService.GetBuddies().Returns(buddies);
        var buddyListVM = new BuddyListViewModel(buddyService);
        await buddyListVM.Init();
        buddyListVM.Buddies.Count.Should().BeGreaterThan(0);

    }

    [Fact]
    public async Task InitShouldNotThrowWhenNoBuddies()
    {
        var buddies = new List<Buddy>();
        var buddyService = Substitute.For<IBuddyService>();
        buddyService.GetBuddies().Returns(buddies);
        var buddyListVM = new BuddyListViewModel(buddyService);
        await buddyListVM.Init();
        Assert.True(buddyListVM.Buddies?.Count == 0);

        Action act = () => buddyListVM.Init();
        act.Should().NotThrow();

    }
}
