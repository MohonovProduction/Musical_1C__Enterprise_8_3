using Moq;
using Presenter;
using Storage;

namespace Musical1C.Tests;

[TestFixture]
public class SoundPresenterTests
{
    private Mock<IStorageDataBase<Sound>> _mockSoundStorage;
    private SoundPresenter _presenter;

    [SetUp]
    public void SetUp()
    {
        _mockSoundStorage = new Mock<IStorageDataBase<Sound>>();
        _presenter = new SoundPresenter(_mockSoundStorage.Object);
    }

    [Test]
    public async Task AddMusicAsync_ShouldAddSoundToStorage()
    {
        // Arrange
        var name = "Symphony No. 9";
        var author = "Beethoven";
        var cancellationToken = CancellationToken.None;

        Sound capturedSound = null!;
        _mockSoundStorage
            .Setup(m => m.AddAsync(It.IsAny<Sound>(), cancellationToken))
            .Callback<Sound, CancellationToken>((sound, token) => capturedSound = sound)
            .Returns(Task.CompletedTask);

        // Act
        var result = await _presenter.AddMusicAsync(name, author, cancellationToken);

        // Assert
        Assert.That(result.Name, Is.EqualTo(name));
        Assert.That(result.Author, Is.EqualTo(author));
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));

        _mockSoundStorage.Verify(m => m.AddAsync(It.IsAny<Sound>(), cancellationToken), Times.Once);
        Assert.That(capturedSound, Is.Not.Null);
        Assert.That(capturedSound.Name, Is.EqualTo(name));
        Assert.That(capturedSound.Author, Is.EqualTo(author));
    }

    [Test]
    public async Task GetMusicAsync_ShouldReturnListOfSounds()
    {
        // Arrange
        var sounds = new List<Sound>
        {
            new Sound(Guid.NewGuid(), "Symphony No. 9", "Beethoven"),
            new Sound(Guid.NewGuid(), "Piano Sonata No. 14", "Beethoven")
        };

        var cancellationToken = CancellationToken.None;

        _mockSoundStorage
            .Setup(m => m.GetListAsync(null!, null!, cancellationToken))
            .ReturnsAsync(sounds);

        // Act
        var result = await _presenter.GetMusicAsync(cancellationToken);

        // Assert
        Assert.That(result, Is.EqualTo(sounds));
        _mockSoundStorage.Verify(m => m.GetListAsync(null!, null!, cancellationToken), Times.Once);
    }
}