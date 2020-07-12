function handleAPI(url, data, reload = false) {
    $.ajax({
        url,
        method: "POST",
        data,
        success: () => {
            if (reload)
                window.location.reload()
            else
                window.location.href = "/Guard/Queue"
        },
        error: () => {
            alert("Lỗi hệ thống!")
        }
    })
}