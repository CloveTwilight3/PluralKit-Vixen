-- schema version 52: 2025-04-24
-- Add system for storing user PluralKit tokens

-- Create the table for storing user PluralKit tokens
CREATE TABLE IF NOT EXISTS user_pluralkit_tokens (
    user_id BIGINT PRIMARY KEY,
    token TEXT NOT NULL,
    system_id UUID,  -- Store the UUID of the linked PK system
    created TIMESTAMP NOT NULL DEFAULT (current_timestamp at time zone 'utc')
);

-- Add index for faster lookups by user_id
CREATE INDEX IF NOT EXISTS user_pluralkit_tokens_user_id_idx ON user_pluralkit_tokens(user_id);

-- Update schema version
update info set schema_version = 52;
