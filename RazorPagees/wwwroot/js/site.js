// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', () =>
{
    const toggle = document.querySelector('.nav-toggle');
    const menu = document.querySelector('.nav-menu');

    if (!toggle || !menu) {
        return;
    }

    toggle.addEventListener('click', () =>
    {
        const isOpen = menu.classList.toggle('is-open');
        toggle.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
    });

    const voteButtons = document.querySelectorAll('.vote-btn');
    voteButtons.forEach(btn =>
    {
        btn.addEventListener('click', async () =>
        {
            const slug = btn.getAttribute('data-slug');
            const direction = btn.getAttribute('data-direction');
            if (!slug || !direction) {
                return;
            }

            try {
                const res = await fetch(`/api/apps/${slug}/vote`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ direction })
                });

                if (!res.ok) {
                    return;
                }

                const data = await res.json();
                const container = btn.parentElement;
                const card = btn.closest('.app-card, .app-detail');
                if (!container || !card) {
                    return;
                }

                const upBtn = container.querySelector('.vote-btn--up span:last-child');
                const downBtn = container.querySelector('.vote-btn--down span:last-child');
                const scoreEl = card.querySelector('.score-value');

                if (upBtn && typeof data.up === 'number') {
                    upBtn.textContent = data.up;
                }

                if (downBtn && typeof data.down === 'number') {
                    downBtn.textContent = data.down;
                }

                if (scoreEl && typeof data.score === 'number') {
                    scoreEl.textContent = data.score;
                }
            } catch (err) {
                console.warn('Vote failed', err);
            }
        });
    });
});
