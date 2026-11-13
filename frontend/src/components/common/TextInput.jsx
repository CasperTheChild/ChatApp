function TextInput({ 
    label,
    name,
    value,
    onChange,
    placeholder,
    type = "text",
    required = false
}) {
    return (
        <div className="mb-3">
            <label htmlFor={name} className="form-label">
                {label}
            </label>

            <input
                className="form-control"
                type={type}
                id={name}
                name={name}
                value={value}
                onChange={onChange}
                placeholder={placeholder}
                required={required}
            />
        </div>
    );
}

export default TextInput;