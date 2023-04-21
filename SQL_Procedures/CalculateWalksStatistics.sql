
CREATE PROCEDURE CalculateWalksStatistics
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
        imei,
        MIN(date_track) AS walk_start,
        MAX(date_track) AS walk_end,
        dbo.distance_between_points(MIN(latitude), MIN(longitude), MAX(latitude), MAX(longitude)) AS walk_distance,
        DATEDIFF(MINUTE, MIN(date_track), MAX(date_track)) AS walk_duration
    FROM
        TrackLocation
    GROUP BY
        imei,
        DATEDIFF(MINUTE, 0, date_track) / 30
    HAVING
        COUNT(*) > 1;

    IF OBJECT_ID('dbo.distance_between_points') IS NULL
    BEGIN
        PRINT 'Error: Function dbo.distance_between_points does not exist'
        RETURN
    END

    SELECT
        DATEPART(YEAR, tl.date_track) AS year,
        DATEPART(MONTH, tl.date_track) AS month,
        DATEPART(DAY, tl.date_track) AS day,
        SUM(walk_distance) AS total_distance,
        SUM(walk_duration) AS total_duration
    FROM
        @walks w
        INNER JOIN TrackLocation tl ON w.imei = tl.imei
    GROUP BY
        DATEPART(YEAR, tl.date_track),
        DATEPART(MONTH, tl.date_track),
        DATEPART(DAY, tl.date_track)
    ORDER BY
        year, month, day;
END