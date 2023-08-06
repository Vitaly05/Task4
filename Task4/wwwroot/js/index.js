const ACCOUNT_STATE_BLOCKED = 0;
const ACCOUNT_STATE_ACTIVE = 1;

$('#select-all-js').change(function () {
    $('.select-user-js').prop('checked', this.checked)
})

$('#block-button-js').click(async e => { 
    e.preventDefault();
    await modalConfirm('Are you sure you want to block selected users?',
        onSuccess = async () => {
            await changeStatus(ACCOUNT_STATE_BLOCKED)
        }
    )
})

$('#unblock-button-js').click(async e => { 
    e.preventDefault();
    await modalConfirm('Are you sure you want to unblock selected users?',
        onSuccess = async () => {
            await changeStatus(ACCOUNT_STATE_ACTIVE)
        }
    )
})

$('#delete-button-js').click(async e => {
    e.preventDefault();
    await modalConfirm('Are you sure you want to delete selected users?',
        onSuccess = async () => {
            await sendUsers('/delete')
        }
    )
})

async function modalConfirm(message, onSuccess) {
    await UIkit.modal.confirm(message).then(async () => {
        await onSuccess()
    }, () => {})
}

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
            UIkit.modal.alert(await response.text())
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