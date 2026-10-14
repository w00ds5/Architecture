using System;

public sealed class GameSaveInteractor : IGameSaveInteractor
{
    private readonly IGameSaveRepository _repository;
    private readonly GameSaveDataValidator _validator;

    public bool HasSave => _repository.HasSave;

    public GameSaveInteractor(
        IGameSaveRepository repository,
        GameSaveDataValidator validator)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));

        _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));
    }

    public void Save(IGameStateSource source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        GameSaveData data = source.Capture();

        _validator.Validate(data);
        _repository.Save(data);
    }

    public GameSaveData Load()
    {
        GameSaveData data = _repository.Load();

        _validator.Validate(data);
        return data;
    }
}