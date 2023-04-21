CREATE PROCEDURE [dbo].[GetTop10WalksForIMEI]
	@imei varchar(50)
AS
BEGIN
DECLARE @walks TABLE (
        walk_id INT PRIMARY KEY IDENTITY(1,1),
        imei VARCHAR(50),
        walk_start DATETIME,
        walk_end DATETIME,
        walk_distance FLOAT,
        walk_duration FLOAT
    );
	INSERT INTO @walks (imei, walk_start, walk_end, walk_distance, walk_duration)
	SELECT
        IMEI,
        MIN(date_track) AS walk_start,
        MAX(date_track) AS walk_end,
        dbo.distance_between_points(MIN(latitude), MIN(longitude), MAX(latitude), MAX(longitude)) AS walk_distance,
        DATEDIFF(MINUTE, MIN(date_track), MAX(date_track)) AS walk_duration
    FROM
        TrackLocation
	WHERE IMEI = @imei
    GROUP BY
        imei,
        DATEDIFF(MINUTE, 0, date_track) / 30
    HAVING
        COUNT(*) > 1
	ORDER BY walk_start DESC;

	SELECT TOP(10) walk_id AS Id, walk_distance, walk_duration
    FROM @walks
    ORDER BY walk_distance DESC;
END