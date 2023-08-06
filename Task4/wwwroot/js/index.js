const ACCOUNT_STATE_BLOCKED = 0;
const ACCOUNT_STATE_ACTIVE = 1;

$('#select-all-js').change(function () {
    $('.select-user-js').prop('checked', this.checked)
})

$('#block-button-js').click(async e => { 
    e.preventDefault();
    await changeStatus(ACCOUNT_STATE_BLOCKED)
});

$('#unblock-button-js').click(async e => { 
    e.preventDefault();
    await changeStatus(ACCOUNT_STATE_ACTIVE)
});

async function changeStatus(status) {
    await sendUsers(`/changeStatus/${status}`)
}

async function sendUsers(url) {
    await fetch(url, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(getSelectedUsersIds())
    }).then(async response => {
        if (response.ok === false) {
            alert(await response.text())
            return
        }
        window.location.href = '/'
    })
}

function getSelectedUsersIds() {
    const ids = $('#users-form-js').serializeArray()
    const f = ids.map(u => u.value)
    return f;
}