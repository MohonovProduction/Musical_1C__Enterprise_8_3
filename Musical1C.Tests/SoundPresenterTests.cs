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
    public class SoundPresenterTests
    {
        private Mock<IStorageDataBase<Sound>> _mockSoundStorage;
        private SoundPresenter _presenter;

        [SetUp]
        public void Setup()
        {
            _mockSoundStorage = new Mock<IStorageDataBase<Sound>>();
            _presenter = new SoundPresenter(_mockSoundStorage.Object);
        }

        [Test]
        public async Task AddMusicAsync_ShouldAddSound()
        {
            // Arrange
            var name = "Song Name";
            var author = "Song Author";
            var token = CancellationToken.None;

            // Act
            var result = await _presenter.AddMusicAsync(name, author, token);

            // Assert
            _mockSoundStorage.Verify(s => s.AddAsync(It.Is<Sound>(
                sound => sound.Name == name && sound.Author == author), token), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(author, result.Author);
        }

        [Test]
        public void AddMusicAsync_ShouldThrowArgumentException_WhenNameIsNullOrWhitespace()
        {
            // Arrange
            var name = "   "; // Whitespace
            var author = "Song Author";
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddMusicAsync(name, author, token));

            Assert.That(exception.Message, Is.EqualTo("Name cannot be null or whitespace. (Parameter 'name')"));
        }

        [Test]
        public void AddMusicAsync_ShouldThrowArgumentException_WhenAuthorIsNullOrWhitespace()
        {
            // Arrange
            var name = "Song Name";
            var author = "   "; // Whitespace
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.AddMusicAsync(name, author, token));

            Assert.That(exception.Message, Is.EqualTo("Author cannot be null or whitespace. (Parameter 'author')"));
        }

        // [Test]
        // public async Task DeleteMusicAsync_ShouldDeleteSound()
        // {
        //     // Arrange
        //     var soundId = Guid.NewGuid();
        //     var token = CancellationToken.None;
        //
        //     // Act
        //     await _presenter.DeleteMusicAsync(soundId, token);
        //
        //     // Assert
        //     _mockSoundStorage.Verify(s => s.DeleteAsync(
        //         It.Is<Func<IQueryable<Sound>, IQueryable<Sound>>>(query =>
        //             query.Invoke(It.IsAny<IQueryable<Sound>>()).Any(s => s.Id == soundId)), token), Times.Once);
        // }

        [Test]
        public void DeleteMusicAsync_ShouldThrowArgumentException_WhenSoundIdIsEmpty()
        {
            // Arrange
            var soundId = Guid.Empty;
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.DeleteMusicAsync(soundId, token));

            Assert.That(exception.Message, Is.EqualTo("Sound ID cannot be empty. (Parameter 'soundId')"));
        }

        [Test]
        public async Task GetMusicAsync_ShouldReturnListFromStorage()
        {
            // Arrange
            var token = CancellationToken.None;
            var sounds = new List<Sound>
            {
                new Sound(Guid.NewGuid(), "Song Name 1", "Author 1"),
                new Sound(Guid.NewGuid(), "Song Name 2", "Author 2")
            };

            _mockSoundStorage.Setup(s => s.GetListAsync(It.IsAny<Func<IQueryable<Sound>, IQueryable<Sound>>>(), token))
                .ReturnsAsync(sounds);

            // Act
            var result = await _presenter.GetMusicAsync(token);

            // Assert
            Assert.AreEqual(sounds, result);
            _mockSoundStorage.Verify(s => s.GetListAsync(It.IsAny<Func<IQueryable<Sound>, IQueryable<Sound>>>(), token), Times.Once);
        }

        [Test]
        public async Task GetMusicByIdAsync_ShouldReturnSound()
        {
            // Arrange
            var soundId = Guid.NewGuid();
            var token = CancellationToken.None;
            var sound = new Sound(soundId, "Song Name", "Song Author");

            _mockSoundStorage.Setup(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Sound>, IQueryable<Sound>>>(), token))
                .ReturnsAsync(sound);

            // Act
            var result = await _presenter.GetMusicByIdAsync(soundId, token);

            // Assert
            Assert.AreEqual(sound, result);
            _mockSoundStorage.Verify(s => s.GetSingleAsync(It.IsAny<Func<IQueryable<Sound>, IQueryable<Sound>>>(), token), Times.Once);
        }

        [Test]
        public void GetMusicByIdAsync_ShouldThrowArgumentException_WhenSoundIdIsEmpty()
        {
            // Arrange
            var soundId = Guid.Empty;
            var token = CancellationToken.None;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _presenter.GetMusicByIdAsync(soundId, token));

            Assert.That(exception.Message, Is.EqualTo("Sound ID cannot be empty. (Parameter 'soundId')"));
        }
    }
}
