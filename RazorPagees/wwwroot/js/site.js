// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener('DOMContentLoaded', () =>
{
    const toggle = document.querySelector('.nav-toggle');
    const menu = document.querySelector('.nav-menu');

    if (toggle && menu) {
        toggle.addEventListener('click', () =>
        {
            const isOpen = menu.classList.toggle('is-open');
            toggle.setAttribute('aria-expanded', isOpen ? 'true' : 'false');
        });
    }

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

        const searchOverlay = document.getElementById('searchOverlay');
        if (searchOverlay) {
            const openers = document.querySelectorAll('[data-open-search]');
            const closeBtn = searchOverlay.querySelector('[data-close-search]');
            const searchField = document.getElementById('searchOverlayInput');
            const searchForm = document.querySelector('[data-search-form]');
            const searchable = Array.from(searchOverlay.querySelectorAll('[data-searchable]'));
            const emptyState = searchOverlay.querySelector('.search-empty');

            const filterResults = (value) =>
            {
                const term = (value || '').toString().trim().toLowerCase();
                let visibleResults = 0;

                searchable.forEach(el =>
                {
                    const haystack = (el.getAttribute('data-searchable') || el.textContent || '').toLowerCase();
                    const matches = !term || haystack.includes(term);
                    const isCategory = el.classList.contains('search-category__item');
                    const isResult = el.classList.contains('search-result');
                    const isChip = el.classList.contains('search-chip');

                    if (isCategory || isResult) {
                        el.classList.toggle('is-hidden', Boolean(term) && !matches);
                        if (isResult && matches) {
                            visibleResults += 1;
                        }
                    } else {
                        el.classList.toggle('is-dimmed', Boolean(term) && !matches);
                    }

                    if (isChip && matches) {
                        visibleResults += 1;
                    }
                });

                if (emptyState) {
                    emptyState.classList.toggle('is-visible', Boolean(term) && visibleResults === 0);
                }
            };

        const openSearch = (prefill) =>
        {
            searchOverlay.classList.add('is-open');
            searchOverlay.setAttribute('aria-hidden', 'false');
            document.body.classList.add('is-locked');

            if (searchField) {
                searchField.value = prefill || '';
                searchField.focus();
                searchField.setSelectionRange(searchField.value.length, searchField.value.length);
                filterResults(searchField.value);
            }
        };

        const closeSearch = () =>
        {
            searchOverlay.classList.remove('is-open');
            searchOverlay.setAttribute('aria-hidden', 'true');
            document.body.classList.remove('is-locked');
            if (searchField) {
                searchField.value = '';
            }
            filterResults('');
        };

        openers.forEach(el =>
        {
            const handleOpen = (evt) =>
            {
                evt.preventDefault();
                const preset = el.getAttribute('data-search-term') || '';
                const value = preset || (el instanceof HTMLInputElement ? el.value : '');
                openSearch(value);
            };

            el.addEventListener('click', handleOpen);
            el.addEventListener('focus', handleOpen);
        });

        if (searchForm) {
            searchForm.addEventListener('submit', (evt) =>
            {
                evt.preventDefault();
                openSearch('');
            });
        }

        closeBtn?.addEventListener('click', closeSearch);

        searchOverlay.addEventListener('click', (evt) =>
        {
            if (evt.target === searchOverlay) {
                closeSearch();
            }
        });

        document.addEventListener('keydown', (evt) =>
        {
            if (evt.key === 'Escape' && searchOverlay.classList.contains('is-open')) {
                closeSearch();
            }
        });

        searchField?.addEventListener('input', (evt) =>
        {
            filterResults(evt.target.value);
        });
    }
});
