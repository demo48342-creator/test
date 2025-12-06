# Database schema draft

Goal: support user-submitted apps, voting, comments, and alternatives. Designed for a relational DB (e.g., PostgreSQL). Includes key columns, relationships, and suggested indexes.

## Tables

### users
- `id` (PK, UUID/int)
- `email` (unique)
- `display_name`
- `avatar_url`
- `role` (enum: user, admin, moderator)
- `password_hash` (or nullable if using SSO)
- `created_at`, `last_seen_at`

### apps
- `id` (PK)
- `slug` (unique, indexed)
- `name`
- `summary` (short text)
- `description` (long/HTML)
- `website_url`
- `logo_url`
- `category`
- `is_new_launch` (bool)
- `golden_kitty_eligible` (bool)
- `launch_date`
- `created_by` (FK → users.id, nullable for seeded data)
- `created_at`, `updated_at`
- Denormalized counters: `upvote_count`, `downvote_count`, `comment_count` (maintained via triggers/jobs)

### app_links (optional if multiple links per app)
- `id` (PK)
- `app_id` (FK → apps.id)
- `label` (e.g., Website, Docs, Pricing, Twitter)
- `url`
- `rank` (int)

### alternatives
- `id` (PK)
- `app_id` (FK → apps.id, the app being viewed)
- `alt_app_id` (FK → apps.id, nullable; set when the alternative is also in the catalog)
- `alt_app_name` (fallback name when `alt_app_id` is null)
- `differentiator`
- `pricing`
- `link_url`
- `logo_url`
- `rank` (ordering)
- `created_at`

### app_tags
- Composite PK `(app_id, tag)`
- `app_id` (FK → apps.id)
- `tag` (text)
- Index on `tag` for search/facets

### submissions (user-submitted apps/workflow)
- `id` (PK)
- `app_id` (FK → apps.id, nullable until approved)
- `submitted_by` (FK → users.id)
- `status` (enum: pending, approved, rejected, changes_requested)
- `payload` (jsonb; captured form including name, slug suggestion, description, links)
- `notes` (moderator notes)
- `created_at`, `reviewed_at`

### votes
- `id` (PK)
- `app_id` (FK → apps.id)
- `user_id` (FK → users.id)
- `direction` (enum: up, down)
- `created_at`
- Unique constraint `(app_id, user_id)` to prevent duplicate votes; allow flipping by updating `direction`.

### comments
- `id` (PK)
- `app_id` (FK → apps.id)
- `user_id` (FK → users.id)
- `parent_id` (FK → comments.id, nullable for threads)
- `body`
- `created_at`, `updated_at`
- `is_deleted` (soft delete)
- Index on `(app_id, created_at)`

### comment_votes (optional)
- `id` (PK)
- `comment_id` (FK → comments.id)
- `user_id` (FK → users.id)
- `direction` (enum: up, down)
- `created_at`
- Unique `(comment_id, user_id)`

### follows (optional notifications)
- `id` (PK)
- `user_id` (FK → users.id)
- `app_id` (FK → apps.id)
- `created_at`

## Notes
- Keep a trigger/job to update `apps.upvote_count`, `downvote_count`, and `comment_count` from `votes` and `comments`.
- Seeded apps can have `created_by` null; user-submitted apps should link to `submissions.submitted_by`.
- `payload` in `submissions` preserves the original submission (name, description, links) even after edits.
- Add a full-text index on `apps.name`, `apps.summary`, `apps.description`, and `app_tags.tag` for search.***
