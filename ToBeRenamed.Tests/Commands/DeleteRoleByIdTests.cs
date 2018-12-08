﻿using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class DeleteRoleByIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DeleteRoleByIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task ItDeletesRoleById()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            const string title = "My Fantastic Library";
            const string roleTitle = "Student";
            const string description = "A suitable description.";

            // User creates library
            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(createLibraryRequest);

            // Get libraries just created by user
            var getLibrariesRequest = new GetLibrariesForUser(user.Id);
            var libraries = await _fixture.SendAsync(getLibrariesRequest);

            // Make sure there's only one library
            var libraryDtos = libraries.ToList();
            Assert.Single(libraryDtos);

            // Get id of the single library
            var libraryId = libraryDtos.ToList().ElementAt(0).Id;

            // User creates role for that library
            var createRoleRequest = new CreateRole(roleTitle, libraryId);
            await _fixture.SendAsync(createRoleRequest);

            // Retrieve that role
            var getRoleRequest = new GetRolesForLibrary(libraryId);
            var role = await _fixture.SendAsync(getRoleRequest);

            // Make sure 1 role returned
            var roleDtos = role.ToList();
            Assert.Single(roleDtos);

            var roleId = roleDtos.ToList().ElementAt(0).Id;

            // Delete that role
            var deleteRoleRequest = new DeleteRoleById(roleId);
            await _fixture.SendAsync(deleteRoleRequest);

            // Try to retrieve that role
            var getRolesLeftRequest = new GetRolesForLibrary(libraryId);
            var roleLeft = await _fixture.SendAsync(getRolesLeftRequest);

            var roleCount = roleLeft.Count();

            Assert.Equal(0, roleCount);
        }
    }
}
