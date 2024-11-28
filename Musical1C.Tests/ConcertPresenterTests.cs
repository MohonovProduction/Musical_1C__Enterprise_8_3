using Moq;
using NUnit.Framework;
using Presenter;
using Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Musical1C.Tests
{
    [TestFixture]
    public class ConcertPresenterTests
    {
        private Mock<IConcertStorage> _mockConcertStorage;
        private Mock<MusicianOnConcertPresenter> _mockMusicianOnConcertPresenter;
        private Mock<SoundOnConcertPresenter> _mockSoundOnConcertPresenter;
        private ConcertPresenter _concertPresenter;

        [SetUp]
        public void SetUp()
        {
            _mockConcertStorage = new Mock<IConcertStorage>();
            _mockMusicianOnConcertPresenter = new Mock<MusicianOnConcertPresenter>();
            _mockSoundOnConcertPresenter = new Mock<SoundOnConcertPresenter>();
            _concertPresenter = new ConcertPresenter(
                _mockConcertStorage.Object,
                _mockMusicianOnConcertPresenter.Object,
                _mockSoundOnConcertPresenter.Object
            );
        }
        
        [Test]
        public async Task AddConcertAsync_ShouldReturnFalse_WhenExceptionOccurs()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var concertName = "Symphony Concert";

            _mockConcertStorage.Setup(x => x.AddConcertAsync(It.IsAny<Concert>(), cancellationToken))
                .Throws(new Exception("Database error"));

            // Act
            var result = await _concertPresenter.AddConcertAsync(concertName, cancellationToken);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SetConcertType_ShouldSetTypeCorrectly()
        {
            // Arrange
            var concertType = "Classical";

            // Act
            _concertPresenter.SetConcertType(concertType);

            // Assert
            var concertBuilder = _concertPresenter.GetConcertBuilderAsync().Result;
            Assert.AreEqual(concertType, concertBuilder.Type);
        }

        [Test]
        public void AddMusicToConcert_ShouldAddMusicCorrectly()
        {
            // Arrange
            var sound = new Sound(Guid.NewGuid(), "Beethoven Symphony", "Beethoven");
            
            // Act
            _concertPresenter.AddMusicToConcert(sound);

            // Assert
            var concertBuilder = _concertPresenter.GetConcertBuilderAsync().Result;
            Assert.Contains(sound, concertBuilder.Music.ToList());
        }

        [Test]
        public void AddMusicianToConcert_ShouldAddMusicianCorrectly()
        {
            // Arrange
            var musician = new Musician(Guid.NewGuid(), "John", "Smith", "Jones");

            // Act
            _concertPresenter.AddMusicianToConcert(musician);

            // Assert
            var concertBuilder = _concertPresenter.GetConcertBuilderAsync().Result;
            Assert.Contains(musician, concertBuilder.Musicians.ToList());
        }

        [Test]
        public async Task DeleteConcertAsync_ShouldDeleteConcertSuccessfully()
        {
            // Arrange
            var concert = new Concert(Guid.NewGuid(), "Symphony Concert", "Classical", "2024-12-25");
            var cancellationToken = CancellationToken.None;
            _mockConcertStorage.Setup(x => x.DeleteConcertAsync(concert, cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            await _concertPresenter.DeleteConcertAsync(concert, cancellationToken);

            // Assert
            _mockConcertStorage.Verify(x => x.DeleteConcertAsync(concert, cancellationToken), Times.Once);
        }

        [Test]
        public void SetConcertDate_ShouldSetDateCorrectly()
        {
            // Arrange
            var date = "2024-12-25";

            // Act
            _concertPresenter.SetConcertDate(date);

            // Assert
            var concertBuilder = _concertPresenter.GetConcertBuilderAsync().Result;
            Assert.AreEqual(date, concertBuilder.Date);
        }

        [Test]
        public async Task GetConcertsAsync_ShouldReturnConcerts()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;
            var concerts = new List<Concert>
            {
                new Concert(Guid.NewGuid(), "Concert 1", "Classical", "2024-12-25"),
                new Concert(Guid.NewGuid(), "Concert 2", "Jazz", "2024-12-26")
            };
            _mockConcertStorage.Setup(x => x.GetAllConcertsAsync(cancellationToken))
                .ReturnsAsync(concerts);

            // Act
            var result = await _concertPresenter.GetConcertsAsync(cancellationToken);

            // Assert
            Assert.AreEqual(concerts.Count, result.Count);
        }
    }
}
