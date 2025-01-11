using Moq;
using NUnit.Framework;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Musical1C.Tests
{
    [TestFixture]
    public class SoundOnConcertPresenterTests
    {
        private Mock<IStorageDataBase<SoundOnConcert>> _mockSoundOnConcertStorage;
        private SoundOnConcertPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockSoundOnConcertStorage = new Mock<IStorageDataBase<SoundOnConcert>>();
            _presenter = new SoundOnConcertPresenter(_mockSoundOnConcertStorage.Object);
        }

        [Test]
        public async Task AddSoundOnConcertAsync_ShouldAddSoundOnConcert()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var soundId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act
            await _presenter.AddSoundOnConcertAsync(concertId, soundId, token);

            // Assert
            _mockSoundOnConcertStorage.Verify(s => s.AddAsync(It.Is<SoundOnConcert>(
                soc => soc.ConcertId == concertId && soc.SoundId == soundId), token), Times.Once);
        }

        [Test]
        public void AddSoundOnConcertAsync_ShouldThrowArgumentException_WhenConcertIdIsEmpty()
        {
            // Arrange
            var concertId = Guid.Empty;
            var soundId = Guid.NewGuid();
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddSoundOnConcertAsync(concertId, soundId, token));

            Assert.That(exception.Message, Is.EqualTo("Concert ID cannot be empty. (Parameter 'concertId')"));
        }

        [Test]
        public void AddSoundOnConcertAsync_ShouldThrowArgumentException_WhenSoundIdIsEmpty()
        {
            // Arrange
            var concertId = Guid.NewGuid();
            var soundId = Guid.Empty;
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddSoundOnConcertAsync(concertId, soundId, token));

            Assert.That(exception.Message, Is.EqualTo("Sound ID cannot be empty. (Parameter 'soundId')"));
        }

        [Test]
        public async Task GetSoundOnConcertAsync_ShouldReturnListFromStorage()
        {
            // Arrange
            var token = CancellationToken.None;
            var soundOnConcerts = new List<SoundOnConcert>
            {
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid()),
                new SoundOnConcert(Guid.NewGuid(), Guid.NewGuid())
            };

            _mockSoundOnConcertStorage
                .Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<SoundOnConcert>, IQueryable<SoundOnConcert>>>(), token))
                .ReturnsAsync(soundOnConcerts);

            // Act
            var result = await _presenter.GetSoundOnConcertAsync(token);

            // Assert
            Assert.AreEqual(soundOnConcerts, result);
            _mockSoundOnConcertStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<SoundOnConcert>, IQueryable<SoundOnConcert>>>(), token), Times.Once);
        }

        // [Test]
        // public async Task DeleteSoundOnConcertAsync_ShouldCallDeleteAsync()
        // {
        //     // Arrange
        //     var concertId = Guid.NewGuid();
        //     var soundId = Guid.NewGuid();
        //     var token = CancellationToken.None;
        //
        //     // Act
        //     await _presenter.DeleteSoundOnConcertAsync(concertId, soundId, token);
        //
        //     // Assert
        //     _mockSoundOnConcertStorage.Verify(s => s.DeleteAsync(
        //         It.Is<Func<IQueryable<SoundOnConcert>, IQueryable<SoundOnConcert>>>(query => 
        //             query.Invoke(It.IsAny<IQueryable<SoundOnConcert>>())
        //                  .Any(s => s.ConcertId == concertId && s.SoundId == soundId)),
        //         token), Times.Once);
        // }
    }
}
