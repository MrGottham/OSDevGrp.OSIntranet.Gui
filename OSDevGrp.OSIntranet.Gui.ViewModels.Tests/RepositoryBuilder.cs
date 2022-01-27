using System;
using AutoFixture.Kernel;
using Moq;
using OSDevGrp.OSIntranet.Gui.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.Gui.ViewModels.Tests
{
    internal static class RepositoryBuilder
    {
        internal static IFinansstyringRepository BuildFinansstyringRepository(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return BuildFinansstyringRepositoryMock(fixture).Object;
        }

        private static Mock<IFinansstyringRepository> BuildFinansstyringRepositoryMock(this ISpecimenBuilder fixture)
        {
            if (fixture == null)
            {
                throw new ArgumentNullException(nameof(fixture));
            }

            return new Mock<IFinansstyringRepository>();
        }
    }
}