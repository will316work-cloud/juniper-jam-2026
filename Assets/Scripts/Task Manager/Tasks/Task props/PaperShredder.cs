using System.Collections.Generic;
using UnityEngine;

public class PaperShredder : TaskProp
{
    public RectTransform PapersParent;
    public PaperSheet PaperPrefab;

    public int MinPapers = 4;
    public int MaxPapers = 8;

    [Range(0, 1)]
    public float BadPaperChance = 0.5f;

    private List<PaperSheet> _papers = new();

    private int _resolved;
    private bool _ended;

    public override void Initialize(GameContext ctx)
    {
        base.Initialize(ctx);
    }

    public override void OnTaskStart()
    {
        ResetTask();

        int count = Random.Range(MinPapers, MaxPapers + 1);

        for (int i = 0; i < count; i++)
        {
            PaperSheet p = Instantiate(PaperPrefab, PapersParent);

            bool signable = Random.value > BadPaperChance;

            p.Initialize(this, signable);

            _papers.Add(p);
        }
    }

    public override void PointerDownHandler() { }

    public override void PointerUpHandler() { }

    public void ResolvePaper()
    {
        if (_ended)
            return;

        _resolved++;

        if (_resolved >= _papers.Count)
        {
            _ended = true;

            _ctx.TaskManager.CurrentTask.OnTaskEnd(true);

            ResetTask();
        }
    }

    public void FailTask()
    {
        if (_ended)
            return;

        _ended = true;

        _ctx.TaskManager.CurrentTask.OnTaskEnd(false);

        ResetTask();
    }

    private void ResetTask()
    {
        foreach (var p in _papers)
        {
            if (p != null)
                Destroy(p.gameObject);
        }

        _papers.Clear();

        _resolved = 0;
        _ended = false;
    }
}